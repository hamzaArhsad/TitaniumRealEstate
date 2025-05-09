# Titanium - Real Estate Website

**Titanium** is a modern, dynamic real estate website designed with Clean Architecture and MVC using ASP.NET Core and C# for the backend. The frontend is built using HTML, CSS, and Bootstrap, providing a responsive and user-friendly design. Key features include real-time property notifications powered by SignalR and an Admin Panel for seamless property listing management.

## Key Features

- **Clean Architecture**: The project follows Clean Architecture principles, ensuring a well-structured and maintainable codebase, with distinct layers for each concern.
  
- **ASP.NET MVC**: Utilizes ASP.NET Core MVC for an efficient and scalable routing system.

- **Responsive Design**: Built with HTML, CSS, and Bootstrap 5 to deliver an optimal experience on desktops, tablets, and mobile devices.

- **Real-Time Notifications**: Powered by SignalR, users receive real-time updates when new properties are listed on the website.

- **Admin Panel**: A secure admin panel allows authorized users to manage property listings, view customer inquiries, and moderate website content.

- **jQuery Integration**: jQuery enhances the user experience with dynamic forms, filters, and content loading.

## Technology Stack

### Backend:
- **ASP.NET Core with MVC**
- **C#**
- **SignalR** for real-time functionality

### Frontend:
- **HTML5 & CSS3**
- **Bootstrap 5** for a responsive UI
- **jQuery** for interactive features

### Database:
- **SQL Server** or any compatible relational database (setup instructions provided)

### Architecture:
- **Clean Architecture**: Divides the application into distinct layers like Presentation, Domain, and Data Access, ensuring maintainability and scalability.

## Real-Time Notifications

The application includes a real-time notification system powered by **SignalR**. When an admin adds a new property, all connected users receive immediate updates to stay informed.

## Admin Panel

To access the admin panel, log in with the provided admin credentials. The admin panel includes the following features:

- **Adding, editing, and deleting properties**
- **Managing user inquiries and accounts**
- **Monitoring system status and real-time notifications**

## Setup Instructions

1. **Clone the repository**:
   ```bash
   git clone https://github.com/hamzaArshad/titanium-real-estate.git
