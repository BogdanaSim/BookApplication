Feature: NSubAllBooks

Performing multiple operations on the book list as an user

@nsub_all_books
Scenario: Adding and deleting a book from an empty list
	Given list of books is empty
	And the user creates a valid book with these details
		| Id | Title             | Authors           | Price | ReleaseDate |
		| 1  | Truth in the Tale | Theodoard Paltiel | 50.8  | 08/01/2022  |
	And the user is redirected to the index page
	And the following book is found in the list
		| Id | Title             | Authors           | Price | ReleaseDate |
		| 1  | Truth in the Tale | Theodoard Paltiel | 50.8  | 08/01/2022  |
	When the user deletes the book with id 1
	Then the user is redirected to the index page
	And list of books is empty
