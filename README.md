# GameStoreHub
Project Overview
Type: Web store (similar in concept to platforms like Steam) for selling digital games.

Before starting the app:
- Please add your default connection string in the appsetting json.
-There is no seeded user, when you enter you need to register from the form.
-If you want to add funds, there is a button in the layout add funds, where you should only add the amount you want.(The other inputs are not included in the controller)

Framework: ASP.NET Core MVC with Entity Framework Core for data management.

Frontend: Bootstrap 3 for responsive design, ensuring the application is mobile-friendly and accessible on various devices.

Architecture
MVC Design: Utilizes the Model-View-Controller (MVC) pattern to separate concerns, making the application easier to manage and scale.
Entity Framework Core: Used for ORM to interact with the database, simplifying data operations like CRUD (Create, Read, Update, Delete).

Core Features
User Accounts:
Integration with ASP.NET Identity for user authentication and authorization.
Features like registration, login, and user profile management. 

Product Management:
You can add, buy games and see their activation codes.

Shopping Cart:
Allows users to add games to their cart and remove items.
Checkout process converts cart items into an order.

Order Management:
Users can view their past orders and details.

Payment System:
Simulated payment interface for adding funds (educational purposes only, no real transactions).

Search Functionality and filters:
Enables users to search for games based on titles or descriptions.
You can filter and also there is pagination.

Security:
Implements measures against common vulnerabilities like XSS.
Ensures secure handling of user data and transactions.

Additional Components
Error Handling:
Custom error pages to handle 404 and other common errors gracefully.

Development and Deployment
Local Development: Using Visual Studio as the primary IDE for development.

Database: SQL Server is used in development, with plans to use the same for production.

Version Control: Utilizes Git for version control, hosting the repository on platforms like GitHub for collaboration and backup.

Admin Login:

Email: admin@gamefinity.com 

Password: 123456

Future Enhancements
Advanced Search: Implement full-text search or use third-party services like Elasticsearch.
Recommendation Engine: Develop a system to recommend games based on user preferences and past purchases.
Internationalization: Prepare the site for multiple languages and currencies to cater to a global audience.
Performance Optimization: Implement caching and other performance enhancements to ensure the site scales well under load.
