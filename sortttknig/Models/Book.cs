using System;

namespace BookCollection.Models
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }
        public int Rating { get; set; }

        public Book()
        {
            Id = Guid.NewGuid().ToString();
            Year = DateTime.Now.Year;
            Rating = 5;
        }
    }
}