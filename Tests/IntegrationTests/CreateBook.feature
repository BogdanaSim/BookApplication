Feature: CreateBook

As a user I want to add new books to the list of books

@tag1
Scenario: Browse Create Book Page
	When the user navigates to the create book page
	Then the Create Book view should be displayed

Scenario: After creating a new book, the user is redirected to the index page
	Given the user is on the create book page
	When the user creates a valid book with these details
		| Title             | Authors           | Price | ReleaseDate |
		| Truth in the Tale | Theodoard Paltiel | 50.8  | 08/01/2022  |
	Then the user is redirected to the index page
	And the book is added to the list

Scenario: After trying create an invalid book, then the validation errors are displayed on the form
	Given the user is on the create book page
	When the user creates a invalid book with these details
		| Title | Authors | Price | ReleaseDate |
		|       |         | 10.1  | 09/10/2020  |
	Then the validation errors are displayed