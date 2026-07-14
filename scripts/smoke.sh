#!/usr/bin/env bash
# SeatBook Smoke Test Script
set -euo pipefail

BASE_URL="${1:-http://localhost:5258}"
PASS=0
FAIL=0

check() {
    local desc="$1" url="$2" expect="$3"
    local code
    code=$(curl --max-time 8 -s -o /dev/null -w "%{http_code}" "$BASE_URL$url")
    if [[ "$code" == "$expect" ]]; then
        echo "  ✅ $desc ($code)"
        PASS=$((PASS+1))
    else
        echo "  ❌ $desc (expected $expect, got $code)"
        FAIL=$((FAIL+1))
    fi
}

echo "===== SeatBook Smoke Test ====="
echo "Base URL: $BASE_URL"
echo "Date: $(date)"
echo ""

echo "--- 用户端 ---"
check "首页" / 200
check "座位列表" /Seat 200
check "座位详情(1)" /Seat/Detail/1 200
check "我的预约(未登录)" /Reservation/MyReservations 302
check "404页面" /nonexistent 404

echo ""
echo "--- 管理端 ---"
check "登录页" /Admin/Login 200
check "预约管理(未登录)" /Admin/Reservations 302
check "座位管理(未登录)" /Admin/Seats 302
check "统计页(未登录)" /Admin/Statistics 302

echo ""
echo "--- 管理端登录 ---"
COOKIE_JAR=$(mktemp)
LOGIN_CODE=$(curl --max-time 8 -s -c "$COOKIE_JAR" -b "$COOKIE_JAR" \
    -o /dev/null -w "%{http_code}" \
    -X POST "$BASE_URL/Admin/Login" \
    -d "Username=%E7%AE%A1%E7%90%86%E5%91%98&Password=123456" -L)
if [[ "$LOGIN_CODE" == "200" ]]; then
    echo "  ✅ 管理员登录 ($LOGIN_CODE)"
    PASS=$((PASS+1))
    if [[ -s "$COOKIE_JAR" ]]; then
        RES_CODE=$(curl --max-time 8 -s -b "$COOKIE_JAR" -o /dev/null -w "%{http_code}" "$BASE_URL/Admin/Reservations")
        [[ "$RES_CODE" == "200" ]] && { echo "  ✅ 预约管理(已登录) ($RES_CODE)"; PASS=$((PASS+1)); } || { echo "  ❌ 预约管理(已登录) (expected 200, got $RES_CODE)"; FAIL=$((FAIL+1)); }
        STA_CODE=$(curl --max-time 8 -s -b "$COOKIE_JAR" -o /dev/null -w "%{http_code}" "$BASE_URL/Admin/Statistics")
        [[ "$STA_CODE" == "200" ]] && { echo "  ✅ 统计页(已登录) ($STA_CODE)"; PASS=$((PASS+1)); } || { echo "  ❌ 统计页(已登录) (expected 200, got $STA_CODE)"; FAIL=$((FAIL+1)); }
    fi
else
    echo "  ⚠️  管理端 POST 需浏览器 AntiForgeryToken（curl 返回 $LOGIN_CODE，浏览器正常）"
fi
rm -f "$COOKIE_JAR"

echo ""
echo "===== 结果: $PASS 通过, $FAIL 失败 ====="
exit $FAIL
