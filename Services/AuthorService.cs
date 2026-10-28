using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using System;
using System.Collections.Generic;

namespace LibraryManagementSystem.Services
{
    public class AuthorService
    {
        private readonly AuthorRepository authorRepository;

        public AuthorService()
        {
            authorRepository = new AuthorRepository();
        }

        // Get all authors
        public List<Author> GetAllAuthors()
        {
            return authorRepository.GetAll();
        }

        // Get one author by ID
        public Author? GetAuthorById(int authorId)
        {
            if (authorId <= 0)
            {
                throw new ArgumentException(
                    "Author ID must be greater than zero.");
            }

            return authorRepository.GetById(authorId);
        }

        // Add a new author
        public int AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            author.FullName = author.FullName.Trim();

            if (string.IsNullOrWhiteSpace(author.FullName))
            {
                throw new ArgumentException(
                    "Author name cannot be empty.");
            }

            if (author.FullName.Length > 100)
            {
                throw new ArgumentException(
                    "Author name cannot exceed 100 characters.");
            }

            if (author.Country != null)
            {
                author.Country = author.Country.Trim();

                if (author.Country.Length == 0)
                {
                    author.Country = null;
                }

                if (author.Country != null && author.Country.Length > 60)
                {
                    throw new ArgumentException(
                        "Country cannot exceed 60 characters.");
                }
            }

            return authorRepository.Add(author);
        }

        // Update an existing author
        public bool UpdateAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            if (author.AuthorId <= 0)
            {
                throw new ArgumentException(
                    "Author ID must be greater than zero.");
            }

            author.FullName = author.FullName.Trim();

            if (string.IsNullOrWhiteSpace(author.FullName))
            {
                throw new ArgumentException(
                    "Author name cannot be empty.");
            }

            if (author.FullName.Length > 100)
            {
                throw new ArgumentException(
                    "Author name cannot exceed 100 characters.");
            }

            if (author.Country != null)
            {
                author.Country = author.Country.Trim();

                if (author.Country.Length == 0)
                {
                    author.Country = null;
                }

                if (author.Country != null && author.Country.Length > 60)
                {
                    throw new ArgumentException(
                        "Country cannot exceed 60 characters.");
                }
            }

            if (authorRepository.GetById(author.AuthorId) == null)
            {
                throw new InvalidOperationException(
                    "Author not found.");
            }

            return authorRepository.Update(author);
        }

        // Delete an author
        public bool DeleteAuthor(int authorId)
        {
            if (authorId <= 0)
            {
                throw new ArgumentException(
                    "Author ID must be greater than zero.");
            }

            if (authorRepository.GetById(authorId) == null)
            {
                throw new InvalidOperationException(
                    "Author not found.");
            }

            return authorRepository.Delete(authorId);
        }
    }
}