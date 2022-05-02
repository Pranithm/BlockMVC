using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_EF_Start.Models
{
    public class Author
    {
        public int AuthorID { get; set; }
        public string AuthorName { get; set; }
        public List<Document>document { get; set; }
    }

    public class Document
    {
        public int DocumentID { get; set; }
        public string ResearchField { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentData { get; set; }
        public DateTime DocumentDate { get; set; }
        public Author author { get; set; }
        public List<Comment> cinfo { get; set; }

    }

    public class Comment
    {

        public int CommentID { get; set; }
        public DateTime CommentDate { get; set; }
        public Document doc { get; set; }
        public User user { get; set; }
        public string CommentData { get; set; }
    }

    public class User
    {
        public int UserID { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public List<Comment> cinfo { get; set; }
    }
}
