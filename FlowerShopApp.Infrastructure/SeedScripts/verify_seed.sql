-- FlowerShopApp seed verification queries

-- A) Expected record counts (minimums because script is non-destructive)
SELECT 'categories' AS table_name, COUNT(*) AS actual_count, 6 AS expected_min FROM categories
UNION ALL
SELECT 'products', COUNT(*), 12 FROM products
UNION ALL
SELECT 'product_images', COUNT(*), 24 FROM product_images
UNION ALL
SELECT 'users', COUNT(*), 2 FROM users
UNION ALL
SELECT 'carts', COUNT(*), 1 FROM carts
UNION ALL
SELECT 'cart_items', COUNT(*), 3 FROM cart_items
UNION ALL
SELECT 'orders', COUNT(*), 2 FROM orders
UNION ALL
SELECT 'order_items', COUNT(*), 4 FROM order_items
UNION ALL
SELECT 'payments', COUNT(*), 2 FROM payments
UNION ALL
SELECT 'chat_rooms', COUNT(*), 2 FROM chat_rooms
UNION ALL
SELECT 'chat_messages', COUNT(*), 4 FROM chat_messages
UNION ALL
SELECT 'notifications', COUNT(*), 2 FROM notifications
UNION ALL
SELECT 'store_locations', COUNT(*), 3 FROM store_locations
ORDER BY table_name;

-- B) Product image integrity: exactly 2 images and exactly 1 primary per seeded product
SELECT
    p.product_id,
    p.product_name,
    COUNT(pi.image_id) AS image_count,
    SUM(CASE WHEN pi.is_primary THEN 1 ELSE 0 END) AS primary_count
FROM products p
LEFT JOIN product_images pi ON pi.product_id = p.product_id
WHERE p.product_id BETWEEN 2001 AND 2012
GROUP BY p.product_id, p.product_name
ORDER BY p.product_id;

-- C) FK integrity checks for seeded ranges (should all be 0)
SELECT 'products_missing_category' AS check_name, COUNT(*) AS orphan_count
FROM products p
LEFT JOIN categories c ON c.category_id = p.category_id
WHERE p.product_id BETWEEN 2001 AND 2012
  AND c.category_id IS NULL
UNION ALL
SELECT 'product_images_missing_product', COUNT(*)
FROM product_images pi
LEFT JOIN products p ON p.product_id = pi.product_id
WHERE pi.image_id BETWEEN 3001 AND 3024
  AND p.product_id IS NULL
UNION ALL
SELECT 'carts_missing_user', COUNT(*)
FROM carts c
LEFT JOIN users u ON u.user_id = c.user_id
WHERE c.cart_id = 5001
  AND u.user_id IS NULL
UNION ALL
SELECT 'cart_items_missing_cart', COUNT(*)
FROM cart_items ci
LEFT JOIN carts c ON c.cart_id = ci.cart_id
WHERE ci.cart_item_id BETWEEN 5101 AND 5103
  AND c.cart_id IS NULL
UNION ALL
SELECT 'cart_items_missing_product', COUNT(*)
FROM cart_items ci
LEFT JOIN products p ON p.product_id = ci.product_id
WHERE ci.cart_item_id BETWEEN 5101 AND 5103
  AND p.product_id IS NULL
UNION ALL
SELECT 'orders_missing_user', COUNT(*)
FROM orders o
LEFT JOIN users u ON u.user_id = o.user_id
WHERE o.order_id BETWEEN 6001 AND 6002
  AND u.user_id IS NULL
UNION ALL
SELECT 'order_items_missing_order', COUNT(*)
FROM order_items oi
LEFT JOIN orders o ON o.order_id = oi.order_id
WHERE oi.order_item_id BETWEEN 6101 AND 6104
  AND o.order_id IS NULL
UNION ALL
SELECT 'order_items_missing_product', COUNT(*)
FROM order_items oi
LEFT JOIN products p ON p.product_id = oi.product_id
WHERE oi.order_item_id BETWEEN 6101 AND 6104
  AND p.product_id IS NULL
UNION ALL
SELECT 'payments_missing_order', COUNT(*)
FROM payments p
LEFT JOIN orders o ON o.order_id = p.order_id
WHERE p.payment_id BETWEEN 7001 AND 7002
  AND o.order_id IS NULL
UNION ALL
SELECT 'chat_rooms_missing_user', COUNT(*)
FROM chat_rooms cr
LEFT JOIN users u ON u.user_id = cr.user_id
WHERE cr.room_id BETWEEN 8001 AND 8002
  AND u.user_id IS NULL
UNION ALL
SELECT 'chat_messages_missing_room', COUNT(*)
FROM chat_messages cm
LEFT JOIN chat_rooms cr ON cr.room_id = cm.room_id
WHERE cm.message_id BETWEEN 8101 AND 8104
  AND cr.room_id IS NULL
UNION ALL
SELECT 'notifications_missing_user', COUNT(*)
FROM notifications n
LEFT JOIN users u ON u.user_id = n.user_id
WHERE n.notification_id BETWEEN 9001 AND 9002
  AND u.user_id IS NULL;

-- D) Seeded account quick view
SELECT user_id, user_name, full_name, role, email, created_at
FROM users
WHERE user_id IN (4001, 4002)
ORDER BY user_id;
