# Shop Application - Full Stack E-Commerce Platform

A modern full-stack e-commerce application built with .NET 8 Web API and React TypeScript

---

## How to Run Locally

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 16+](https://nodejs.org/) and npm
- SQL Server (localdb is used by default)
- Git

---

### Backend Setup

1. **Navigate to backend directory:**
   ```sh
   cd backend
   ```

2. **Restore NuGet packages:**
   ```sh
   dotnet restore
   ```

3. **Database Setup & Seed Data:**
   - Connection string is set in `appsettings.Development.json`:
   
   - Seed data is automatically applied via EF Core migrations.
   - Run migrations:
     ```sh
     dotnet ef database update
     ```

4. **Environment Variables:**
   - JWT secret key is set in `appsettings.Development.json`:

   - You can override config values with environment variables if needed.

5. **Start the API:**
   ```sh
   dotnet run
   ```
   - The API will be available at `http://localhost:5229`
   - Swagger UI: `http://localhost:5229/swagger`

---

### Frontend Setup

1. **Navigate to frontend directory:**
   ```sh
   cd frontend
   ```

2. **Install npm packages:**
   ```sh
   npm install
   ```

3. **Environment Variables:**
   - REACT_APP_API_URL is set in `.env.local` 
     ```
     REACT_APP_API_URL=http://localhost:5229/api
     ```
   - This configures the API base URL for local development.

4. **Start the React development server:**
   ```sh
   npm start
   ```
   - The frontend will be available at `http://localhost:3000`

---

## Architecture Decisions & Trade-Offs

### State Management

- **Zustand** is used for state management (auth, cart, products) because it is lightweight, easy to use, and has excellent TypeScript support.
- **Trade-off:** Zustand is simpler than Redux, but lacks some advanced middleware and devtools integrations. For this project’s scale, simplicity and performance are prioritized.

### Paging Approach

- **Backend:** Paging is implemented via API endpoints using `PaginationRequest` and `PagedResult` DTOs.
- **Frontend:** Infinite scroll loads more products as the user scrolls, reducing initial load time and improving UX.
- **Trade-off:** Infinite scroll is user-friendly but can make navigation/bookmarking harder compared to classic pagination.

### Cart Management

- **Hybrid Approach:** Guests use localStorage for cart; authenticated users use API for persistent cart.
- **Transition:** Cart syncs automatically when a user logs in (from localStorage to API, then the localStorage gets cleaned).
- **Trade-off:** Requires careful sync logic to avoid conflicts, but provides a smooth experience for both guests and users.

### Error Handling

- **Result Pattern:** Consistent error handling in backend services.
- **Error Boundaries:** React error boundaries catch and display errors gracefully.

### API Design

- **RESTful Endpoints:** Standard HTTP methods and status codes.
- **Type Safety:** Matching DTOs between frontend and backend.
- **Swagger/OpenAPI:** Automatic API documentation.

---

## What’s Next (Future Enhancements)

With more time, I would implement:

### Backend Improvements

- **Order Management:** Checkout flow and order tracking.
- **Image Uploads:** File storage for product images.
- **Advanced Search & Filtering:** Full-text search, better filtering logic for production.
- **User Data Management:** Disabling account, user data editing.


### Frontend Enhancements

- **Product Reviews:** User ratings and reviews.
- **Wishlist:** Save products for later.
- **Order History:** View past orders and tracking.
- **Product Window And Quantity:** View product information in seperate pop-up window, add product quantity for more realistic implementation and management.
- **Better UI:** Make the UI more aesthetic and the navigation more intuitive.

### DevOps & Deployment

- **Dockerization:** Containerize backend and frontend for easy deployment.
- **Monitoring & Logging:** Centralized logging and performance monitoring.
---