Introduce a Transaction table that logs all quote/booking/fee events and their statuses for auditing and billing purposes.

### Tasks:

- [ ] Create Transaction model and corresponding table
- [ ] Include fields like:
  - Id
  - ClientId
  - Amount
  - Currency
  - Status
  - BookingReference
  - Timestamp
- [ ] Link to AvaClient via FK
- [ ] Support success/failure/pending statuses
- [ ] Seed with sample data or backfill logic
