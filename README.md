# Short Links Project
This is a simple URL shortener using ASP.NET Core and Entity Framework Core. used Identity to manage user authentication and authorization, and implemented external authentication with Google. also created a simple API for creating and retrieving shortened links.

Overall, this project demonstrates how to build a basic web application using ASP.NET Core and some common libraries. It can be extended and customized in many ways to fit a variety of needs.

## API
The API contains three functions:

### POST Function: /api/cutter
A POST method that takes a string parameter originalUrl in the request body. It checks if the URL is valid and creates a short link in the link database with the CreateShortLink method. If the user is authenticated, the action associates the link with the user ID. Finally, the method returns the short link in the response body.

### GET Function: /W/{shortURL}
A GET method that takes a string parameter shortURL in the route. It retrieves the original URL associated with the short link from the link database using the GiveOriginalLink method and redirects the user to the original URL.

### GET Function: /W/Links
A GET method that takes a string parameter userID in the route. It retrieves all the links associated with the user ID from the link database using the AllLinksByID method and returns them in the response body.

Together, these actions provide a complete implementation of a URL shortening API with the ability to create, access, and view links associated with user accounts.

## Google Authentication
The Google Authentication contains three functions:

### Login Function: ExternalLogin
Initiates the authentication process by configuring external authentication properties for the "Google" provider and redirecting the user to the Google authentication page.

### Callback Function: ExternalLoginCallback
Handles the response from Google and authenticates the user if the response is valid. It checks if the user has already signed in with Google before and, if so, retrieves the user information and signs the user in. If the user is new, the method creates a new user account and signs the user in.

### Logout Function: ExternalLogout
Signs the user out of the application by calling the SignOutAsync() method of the SignInManager object and the SignOutAsync() method of the HttpContext object with the external authentication scheme. Finally, the method redirects the user to the "Index" action of the "Home" controller.

Together, these actions provide a complete implementation of external authentication with Google, allowing users to log in to the application using their Google accounts and log out when they are finished.

## HomeController
The HomeController contains the following actions:

### Index()
Returns the Index view.

### LinksByUser()
An asynchronous action that retrieves all the links associated with the authenticated user ID using the AllLinksByID method of the API. If the user is not authenticated, the method returns the Access Denied view. The links are returned in the LinksByUser view.

### Error()
Returns the Error view with a RequestId if there is an error.

Together, these actions provide the functionality for the main pages of the application.