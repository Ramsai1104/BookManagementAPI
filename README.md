# BookManagementAPI
Web API test sample with Book Management System

# About BookManagementAPI

This REST API is built using .Net 4.6.1 framwork, ASP.NET Web API 2

This repository contains a controller which is dealing with Books with their details. You can GET/POST/PUT/PATCH and DELETE them.
Hope this helps.

REST API request response can be tested using Swagger which is attached with this application

## Swagger Url with few demonstrated examples

``` http://localhost:63272/swagger ```

![BooksManagementAPI](https://github.com/Ramsai1104/BookManagementAPI/blob/master/versions.jpg.PNG)

## GET all books

``` http://localhost:63272/api/v1/books ```

![BooksManagementAPI](https://github.com/Ramsai1104/BookManagementAPI/blob/master/getAllBooks.PNG)

## GET single book by book id

``` http://localhost:63272/api/v1/books/bk104 ```

![BooksManagementAPI](https://github.com/Ramsai1104/BookManagementAPI/blob/master/getSingle.PNG)

## GET books by Author

``` http://localhost:63272/api/books/Kalam ```

![BooksManagementAPI](https://github.com/Ramsai1104/BookManagementAPI/blob/master/getBooksByAuthor.PNG)

## GET books by Price Range

``` http://localhost:63272/api/books/100/500 ```

![BooksManagementAPI](https://github.com/Ramsai1104/BookManagementAPI/blob/master/getBooksByPriceRange.PNG)

## [POST] Add New books

```javascript
    {
        "Id": 205,
        "BookName": "Famous Five Adventures",
        "Author": "Enid Bliton",
        "PublishedDate": "1999-11-14",
        "Price": 1104.35
    }
```

![BooksManagementAPI](https://github.com/Ramsai1104/BookManagementAPI/blob/master/addNewBook.PNG)



