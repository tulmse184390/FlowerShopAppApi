# Seed Scripts (PostgreSQL)

This folder contains non-destructive SQL scripts to seed demo data for FlowerShopApp.

## Files

- `seed_all.sql`: Main script that inserts all sample data and synchronizes ID sequences.
- `verify_seed.sql`: Validation queries for counts, image constraints, and FK integrity.

## Seed Scope

The seed includes:

- 6 categories
- 12 products
- 24 product images (2 per product, exactly 1 primary image)
- 2 users (admin + customer)
- 1 active cart + cart items
- 2 orders + order items + payments
- 2 chat rooms + chat messages
- 2 notifications
- 3 store locations

## Important Notes

- The script does not use drop/truncate/delete operations.
- IDs are in high ranges to reduce collisions with existing data.
- Product image links use public demo URLs (replace with your own CDN links if needed).
- `seed_all.sql` attempts to enable `pgcrypto` and use bcrypt (`crypt`) for user passwords.
- If `pgcrypto` is unavailable and cannot be enabled, password hashes fall back to plain text values.

## Seeded Accounts

- Username: `admin123`
- Password: `Admin@123`
- Role: `ADMIN`

- Username: `user123`
- Password: `User@123`
- Role: `USER`

If login fails for seeded accounts, verify that `pgcrypto` is enabled so bcrypt hashes are generated.

## Prerequisite

Apply all EF migrations before running seed scripts.

```bash
dotnet ef database update --project FlowerShopApp.Infrastructure --startup-project FlowerShopApp.Api
```

## Run With psql

```bash
psql "host=localhost port=5432 dbname=flower_app_db user=postgres password=12345" -f "FlowerShopApp.Infrastructure/SeedScripts/seed_all.sql"
psql "host=localhost port=5432 dbname=flower_app_db user=postgres password=12345" -f "FlowerShopApp.Infrastructure/SeedScripts/verify_seed.sql"
```

## Run With pgAdmin

1. Open Query Tool for database `flower_app_db`.
2. Execute `seed_all.sql`.
3. Execute `verify_seed.sql`.
4. Confirm expected counts and zero orphan rows from output.

## API Smoke Test Suggestions

After seeding, test these endpoints:

- `POST /api/auth/login`
- `GET /api/products`
- `GET /api/products/{id}`
- `GET /api/store`
- `GET /api/orders` (with user token)
- `GET /api/cart` (with user token)
