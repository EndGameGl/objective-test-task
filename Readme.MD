# Test task for Objective

## Launching project
- `cd` into root folder
- `docker compose up -d` to boot up the project
- API will be available at http://localhost:15456

## Public endpoints
| Endpoint | Method |  Parameters | Description |
|----------|--------|-------------|-------------|
| `/api/v1/subscriptions/prinzip/subscribe` | POST | `{ mail: String, url: String }`  | Attempts to add new subscription to check for price changes on provided apartement |
| `/api/v1/subscriptions/prinzip/` | GET | `mail: String` | Provides current information for all tracked apartments, which provided mail is subscribed to |

## Tools
- PostgreSQL is available for connection at `localhost:5432`, connection string can be copied from `appsettings.json`
- Mailpit is available at http://localhost:8025/ , which can be checked to see new mails coming in when prices update

## Examples
- To see parser in action, follow next steps:
 1. Add to subscription with `/api/v1/subscriptions/prinzip/subscribe` endpoint
 2. Within 5 seconds initial data will be filled automatically
 3. Change the price in database directly
 4. Within 5 seconds mail will be sent with pricing information

## Overall process
- `PrinzipSubscriptionsController` handles the request
- `PrinzipSubscriptionHandler` saves all related data to database using `PriceTrackerDbContext`
- `BackgroundPriceUpdater` works in background on an interval timers
 1. All current price trackers are loaded from the database
 2. `PrinzipApiClient` is called to get current pricing data
 3. Data is compared
 4. If changes are found, email requests are sent to `BackgroundMailSender`
- `BackgroundMailSender` listens for new mail requests and sends them to respective addresses

## What could be improved:
- More static data, such as subscriber info could be cached, InMemory or Redis

## 2nd task (concurrent value incrementing)
- Current solution relies on `ReaderWriterLockSlim` for simplicity and readability
- Test console project can be launched to see that Parallel execution works correctly and final value is correct