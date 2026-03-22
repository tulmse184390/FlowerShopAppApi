-- FlowerShopApp demo seed (PostgreSQL)
-- Non-destructive: uses INSERT ... SELECT ... WHERE NOT EXISTS
-- Safe to run multiple times.

BEGIN;

-- Optional extension for bcrypt hashing in SQL.
DO $$
BEGIN
    CREATE EXTENSION IF NOT EXISTS pgcrypto;
EXCEPTION
    WHEN insufficient_privilege THEN
        RAISE NOTICE 'Skipping pgcrypto extension creation due to insufficient privileges.';
END $$;

-- 1) Categories (6)
INSERT INTO categories (category_id, category_name, description)
SELECT v.category_id, v.category_name, v.description
FROM (
    VALUES
        (1001, 'Bouquets', 'Hand-tied bouquets for birthdays, anniversaries, and everyday gifting.'),
        (1002, 'Roses', 'Classic roses in multiple colors and arrangements.'),
        (1003, 'Lilies', 'Elegant lily arrangements for modern spaces and events.'),
        (1004, 'Wedding Flowers', 'Curated floral sets for weddings and receptions.'),
        (1005, 'Sympathy Flowers', 'Respectful arrangements for condolences and memorials.'),
        (1006, 'Plants & Gifts', 'Potted plants and gift-ready floral add-ons.')
) AS v(category_id, category_name, description)
WHERE NOT EXISTS (
    SELECT 1 FROM categories c WHERE c.category_id = v.category_id
);

-- 2) Products (12)
INSERT INTO products
    (product_id, product_name, brief_description, full_description, price, stock_quantity, category_id, created_at, is_deleted, deleted_at)
SELECT
    v.product_id,
    v.product_name,
    v.brief_description,
    v.full_description,
    v.price,
    v.stock_quantity,
    v.category_id,
    CURRENT_TIMESTAMP,
    FALSE,
    NULL
FROM (
    VALUES
        (2001, 'Sunrise Mixed Bouquet', 'Bright mixed blooms with warm tones.', 'A cheerful bouquet featuring gerbera, carnations, and seasonal fillers in sunrise-inspired colors.', 28.99::numeric(18,2), 30, 1001),
        (2002, 'Pastel Dream Bouquet', 'Soft pastel hand-tied bouquet.', 'A gentle mix of peach roses, white chrysanthemums, and eucalyptus for a calming look.', 31.50::numeric(18,2), 24, 1001),
        (2003, 'Classic Red Roses (12)', 'A dozen premium red roses.', 'Twelve long-stem red roses wrapped with greenery and ribbon.', 39.99::numeric(18,2), 40, 1002),
        (2004, 'Pink Rose Box', 'Elegant pink roses in gift box.', 'A luxury-style arrangement of pink roses presented in a compact gift box.', 42.00::numeric(18,2), 18, 1002),
        (2005, 'White Lily Harmony', 'Fresh white lilies with foliage.', 'Fragrant white lilies arranged with premium greens, suitable for home and office.', 36.50::numeric(18,2), 20, 1003),
        (2006, 'Oriental Lily Vase', 'Ornamental lilies in a glass vase.', 'Tall oriental lilies pre-arranged in a reusable clear glass vase.', 45.00::numeric(18,2), 12, 1003),
        (2007, 'Bridal Bouquet - Ivory', 'Ivory-toned bridal bouquet.', 'Wedding bouquet with roses, lisianthus, and baby''s breath in ivory palette.', 79.00::numeric(18,2), 8, 1004),
        (2008, 'Wedding Table Centerpiece', 'Floral centerpiece for reception tables.', 'Low-profile centerpiece with roses and hydrangea for wedding decor themes.', 55.00::numeric(18,2), 15, 1004),
        (2009, 'Peaceful White Basket', 'Sympathy flower basket.', 'A respectful white-themed sympathy basket with lilies and chrysanthemums.', 15.99::numeric(18,2), 14, 1005),
        (2010, 'Condolence Standing Spray', 'Standing sympathy arrangement.', 'Large standing spray arrangement designed for memorial ceremonies.', 95.00::numeric(18,2), 6, 1005),
        (2011, 'Lucky Bamboo Pot', 'Easy-care indoor lucky bamboo.', 'A compact lucky bamboo arrangement in ceramic pot, ideal as a gift.', 18.50::numeric(18,2), 35, 1006),
        (2012, 'Mini Succulent Gift Set', 'Set of 3 mini succulents.', 'Three assorted mini succulents in decorative pots with gift-ready packaging.', 22.00::numeric(18,2), 28, 1006)
) AS v(product_id, product_name, brief_description, full_description, price, stock_quantity, category_id)
WHERE NOT EXISTS (
    SELECT 1 FROM products p WHERE p.product_id = v.product_id
);

-- 3) Product images (24 = 2 per product, exactly one primary each)
INSERT INTO product_images (image_id, product_id, image_url, is_primary)
SELECT v.image_id, v.product_id, v.image_url, v.is_primary
FROM (
    VALUES
        (3001, 2001, 'https://images.pexels.com/photos/931177/pexels-photo-931177.jpeg', TRUE),
        (3002, 2001, 'https://images.pexels.com/photos/954877/pexels-photo-954877.jpeg', FALSE),
        (3003, 2002, 'https://images.pexels.com/photos/90946/pexels-photo-90946.jpeg', TRUE),
        (3004, 2002, 'https://images.pexels.com/photos/1207978/pexels-photo-1207978.jpeg', FALSE),
        (3005, 2003, 'https://images.pexels.com/photos/56866/garden-rose-red-pink-56866.jpeg', TRUE),
        (3006, 2003, 'https://images.pexels.com/photos/66274/rose-bloom-red-blossom-66274.jpeg', FALSE),
        (3007, 2004, 'https://images.pexels.com/photos/1464206/pexels-photo-1464206.jpeg', TRUE),
        (3008, 2004, 'https://images.pexels.com/photos/175389/pexels-photo-175389.jpeg', FALSE),
        (3009, 2005, 'https://images.pexels.com/photos/106936/pexels-photo-106936.jpeg', TRUE),
        (3010, 2005, 'https://images.pexels.com/photos/2382325/pexels-photo-2382325.jpeg', FALSE),
        (3011, 2006, 'https://images.pexels.com/photos/1903962/pexels-photo-1903962.jpeg', TRUE),
        (3012, 2006, 'https://images.pexels.com/photos/1005718/pexels-photo-1005718.jpeg', FALSE),
        (3013, 2007, 'https://images.pexels.com/photos/265722/pexels-photo-265722.jpeg', TRUE),
        (3014, 2007, 'https://images.pexels.com/photos/2253870/pexels-photo-2253870.jpeg', FALSE),
        (3015, 2008, 'https://images.pexels.com/photos/931162/pexels-photo-931162.jpeg', TRUE),
        (3016, 2008, 'https://images.pexels.com/photos/1070850/pexels-photo-1070850.jpeg', FALSE),
        (3017, 2009, 'https://images.pexels.com/photos/1022922/pexels-photo-1022922.jpeg', TRUE),
        (3018, 2009, 'https://images.pexels.com/photos/1787436/pexels-photo-1787436.jpeg', FALSE),
        (3019, 2010, 'https://images.pexels.com/photos/1187079/pexels-photo-1187079.jpeg', TRUE),
        (3020, 2010, 'https://images.pexels.com/photos/931168/pexels-photo-931168.jpeg', FALSE),
        (3021, 2011, 'https://images.pexels.com/photos/5699665/pexels-photo-5699665.jpeg', TRUE),
        (3022, 2011, 'https://images.pexels.com/photos/4503273/pexels-photo-4503273.jpeg', FALSE),
        (3023, 2012, 'https://images.pexels.com/photos/1084199/pexels-photo-1084199.jpeg', TRUE),
        (3024, 2012, 'https://images.pexels.com/photos/4503751/pexels-photo-4503751.jpeg', FALSE)
) AS v(image_id, product_id, image_url, is_primary)
WHERE NOT EXISTS (
    SELECT 1 FROM product_images pi WHERE pi.image_id = v.image_id
);

-- 4) Users (2)
-- If pgcrypto is available: use bcrypt via crypt(..., gen_salt('bf')).
-- If not: fallback to plain text so data still seeds; login hashing checks may fail in that case.
INSERT INTO users
    (user_id, user_name, password_hash, full_name, email, phone_number, address, role, created_at)
SELECT
    v.user_id,
    v.user_name,
    CASE
        WHEN to_regprocedure('crypt(text,text)') IS NOT NULL
            THEN crypt(v.plain_password, gen_salt('bf'))
        ELSE v.plain_password
    END,
    v.full_name,
    v.email,
    v.phone_number,
    v.address,
    v.role,
    CURRENT_TIMESTAMP
FROM (
    VALUES
        (4001, 'admin123', 'Admin@123', 'System Administrator', 'admin@flowershop.local', '0900000001', '123 Nguyen Hue, District 1, Ho Chi Minh City', 'ADMIN'),
        (4002, 'user123', 'User@123', 'Demo Customer', 'user@flowershop.local', '0900000002', '45 Le Loi, District 1, Ho Chi Minh City', 'USER')
) AS v(user_id, user_name, plain_password, full_name, email, phone_number, address, role)
WHERE NOT EXISTS (
    SELECT 1 FROM users u WHERE u.user_id = v.user_id
);

-- 5) Cart + cart items
INSERT INTO carts (cart_id, user_id, status, created_at)
SELECT 5001, 4002, 'ACTIVE', CURRENT_TIMESTAMP
WHERE NOT EXISTS (SELECT 1 FROM carts c WHERE c.cart_id = 5001);

INSERT INTO cart_items (cart_item_id, cart_id, product_id, quantity, price)
SELECT v.cart_item_id, v.cart_id, v.product_id, v.quantity, v.price
FROM (
    VALUES
        (5101, 5001, 2001, 1, 28.99::numeric(18,2)),
        (5102, 5001, 2005, 2, 36.50::numeric(18,2)),
        (5103, 5001, 2012, 1, 22.00::numeric(18,2))
) AS v(cart_item_id, cart_id, product_id, quantity, price)
WHERE NOT EXISTS (
    SELECT 1 FROM cart_items ci WHERE ci.cart_item_id = v.cart_item_id
);

-- 6) Orders + order items + payments
INSERT INTO orders
    (order_id, user_id, total_amount, payment_method, shipping_address, order_status, order_date)
SELECT v.order_id, v.user_id, v.total_amount, v.payment_method, v.shipping_address, v.order_status, CURRENT_TIMESTAMP
FROM (
    VALUES
        (6001, 4002, 101.99::numeric(18,2), 'COD', '45 Le Loi, District 1, Ho Chi Minh City', 'DELIVERED'),
        (6002, 4002, 37.99::numeric(18,2), 'VNPAY', '45 Le Loi, District 1, Ho Chi Minh City', 'CONFIRMED')
) AS v(order_id, user_id, total_amount, payment_method, shipping_address, order_status)
WHERE NOT EXISTS (
    SELECT 1 FROM orders o WHERE o.order_id = v.order_id
);

INSERT INTO order_items
    (order_item_id, order_id, product_id, product_name, quantity, price)
SELECT v.order_item_id, v.order_id, v.product_id, v.product_name, v.quantity, v.price
FROM (
    VALUES
        (6101, 6001, 2001, 'Sunrise Mixed Bouquet', 1, 28.99::numeric(18,2)),
        (6102, 6001, 2005, 'White Lily Harmony', 2, 36.50::numeric(18,2)),
        (6103, 6002, 2009, 'Peaceful White Basket', 1, 15.99::numeric(18,2)),
        (6104, 6002, 2012, 'Mini Succulent Gift Set', 1, 22.00::numeric(18,2))
) AS v(order_item_id, order_id, product_id, product_name, quantity, price)
WHERE NOT EXISTS (
    SELECT 1 FROM order_items oi WHERE oi.order_item_id = v.order_item_id
);

INSERT INTO payments
    (payment_id, order_id, amount, payment_gateway, payment_status, payment_date)
SELECT v.payment_id, v.order_id, v.amount, v.payment_gateway, v.payment_status, CURRENT_TIMESTAMP
FROM (
    VALUES
        (7001, 6001, 101.99::numeric(18,2), 'CASH', 'PAID'),
        (7002, 6002, 37.99::numeric(18,2), 'VNPAY', 'SUCCESS')
) AS v(payment_id, order_id, amount, payment_gateway, payment_status)
WHERE NOT EXISTS (
    SELECT 1 FROM payments p WHERE p.payment_id = v.payment_id
);

-- 7) Chat rooms + chat messages
INSERT INTO chat_rooms (room_id, user_id, created_at, is_ai_assisted)
SELECT v.room_id, v.user_id, CURRENT_TIMESTAMP, v.is_ai_assisted
FROM (
    VALUES
        (8001, 4002, TRUE),
        (8002, 4001, TRUE)
) AS v(room_id, user_id, is_ai_assisted)
WHERE NOT EXISTS (
    SELECT 1 FROM chat_rooms cr WHERE cr.room_id = v.room_id
);

INSERT INTO chat_messages (message_id, room_id, sender_role, message, sent_at)
SELECT v.message_id, v.room_id, v.sender_role, v.message, CURRENT_TIMESTAMP
FROM (
    VALUES
        (8101, 8001, 'USER', 'Hi, I need flowers for an anniversary dinner.'),
        (8102, 8001, 'AI', 'Great choice! Red roses and lilies are perfect for anniversaries.'),
        (8103, 8002, 'USER', 'Please check low stock products for this week.'),
        (8104, 8002, 'AI', 'Current low stock: Bridal Bouquet - Ivory and Condolence Standing Spray.')
) AS v(message_id, room_id, sender_role, message)
WHERE NOT EXISTS (
    SELECT 1 FROM chat_messages cm WHERE cm.message_id = v.message_id
);

-- 8) Notifications
INSERT INTO notifications (notification_id, user_id, message, is_read, created_at)
SELECT v.notification_id, v.user_id, v.message, v.is_read, CURRENT_TIMESTAMP
FROM (
    VALUES
        (9001, 4002, 'Your order #6002 has been confirmed.', FALSE),
        (9002, 4001, 'New customer chat started in room #8001.', TRUE)
) AS v(notification_id, user_id, message, is_read)
WHERE NOT EXISTS (
    SELECT 1 FROM notifications n WHERE n.notification_id = v.notification_id
);

-- 9) Store locations
INSERT INTO store_locations (location_id, store_name, latitude, longitude, address)
SELECT v.location_id, v.store_name, v.latitude, v.longitude, v.address
FROM (
    VALUES
        (9501, 'FlowerShop - District 1', 10.776530::numeric(9,6), 106.700981::numeric(9,6), '12 Dong Khoi, District 1, Ho Chi Minh City'),
        (9502, 'FlowerShop - District 3', 10.785965::numeric(9,6), 106.682251::numeric(9,6), '245 Vo Thi Sau, District 3, Ho Chi Minh City'),
        (9503, 'FlowerShop - Thu Duc', 10.850632::numeric(9,6), 106.771900::numeric(9,6), '1 Vo Van Ngan, Thu Duc City, Ho Chi Minh City')
) AS v(location_id, store_name, latitude, longitude, address)
WHERE NOT EXISTS (
    SELECT 1 FROM store_locations sl WHERE sl.location_id = v.location_id
);

-- 10) Sequence synchronization
SELECT setval(pg_get_serial_sequence('categories', 'category_id'), GREATEST((SELECT COALESCE(MAX(category_id), 1) FROM categories), 1), true);
SELECT setval(pg_get_serial_sequence('products', 'product_id'), GREATEST((SELECT COALESCE(MAX(product_id), 1) FROM products), 1), true);
SELECT setval(pg_get_serial_sequence('product_images', 'image_id'), GREATEST((SELECT COALESCE(MAX(image_id), 1) FROM product_images), 1), true);
SELECT setval(pg_get_serial_sequence('users', 'user_id'), GREATEST((SELECT COALESCE(MAX(user_id), 1) FROM users), 1), true);
SELECT setval(pg_get_serial_sequence('carts', 'cart_id'), GREATEST((SELECT COALESCE(MAX(cart_id), 1) FROM carts), 1), true);
SELECT setval(pg_get_serial_sequence('cart_items', 'cart_item_id'), GREATEST((SELECT COALESCE(MAX(cart_item_id), 1) FROM cart_items), 1), true);
SELECT setval(pg_get_serial_sequence('orders', 'order_id'), GREATEST((SELECT COALESCE(MAX(order_id), 1) FROM orders), 1), true);
SELECT setval(pg_get_serial_sequence('order_items', 'order_item_id'), GREATEST((SELECT COALESCE(MAX(order_item_id), 1) FROM order_items), 1), true);
SELECT setval(pg_get_serial_sequence('payments', 'payment_id'), GREATEST((SELECT COALESCE(MAX(payment_id), 1) FROM payments), 1), true);
SELECT setval(pg_get_serial_sequence('chat_rooms', 'room_id'), GREATEST((SELECT COALESCE(MAX(room_id), 1) FROM chat_rooms), 1), true);
SELECT setval(pg_get_serial_sequence('chat_messages', 'message_id'), GREATEST((SELECT COALESCE(MAX(message_id), 1) FROM chat_messages), 1), true);
SELECT setval(pg_get_serial_sequence('notifications', 'notification_id'), GREATEST((SELECT COALESCE(MAX(notification_id), 1) FROM notifications), 1), true);
SELECT setval(pg_get_serial_sequence('store_locations', 'location_id'), GREATEST((SELECT COALESCE(MAX(location_id), 1) FROM store_locations), 1), true);

COMMIT;
