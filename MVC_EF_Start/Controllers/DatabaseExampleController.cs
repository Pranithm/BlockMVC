using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_EF_Start.DataAccess;
using MVC_EF_Start.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVC_EF_Start.Controllers
{
  public class DatabaseExampleController : Controller
  {
        HttpClient httpClient;

        static string BASE_URL = "https://data.messari.io/api/v1";
        public ApplicationDbContext dbContext;

    public DatabaseExampleController(ApplicationDbContext context)
    {
      dbContext = context;
    }

    public async Task<ViewResult> Index()
    {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
           // httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string Block_API_PATH = BASE_URL + "/news";
            string blockData = "";

            

            httpClient.BaseAddress = new Uri(Block_API_PATH);

            try
            {
         
                HttpResponseMessage response = httpClient.GetAsync(Block_API_PATH)
                                                        .GetAwaiter().GetResult();



                if (response.IsSuccessStatusCode)
                {
                    blockData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!blockData.Equals(""))
                {
                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    var temp = JObject.Parse(blockData);
                    var  blocks = JsonConvert.DeserializeObject(blockData);
            
                    var z=temp["data"].ToList();
                    // converting the data to JSON format using newtonJSON
                    foreach(var x in z)
                    {
                        var docTitle = x["title"].ToString();
                        var docData = x["content"].ToString().Substring(0,800);
                        var author = x["author"]["name"].ToString();
                        DateTime pubDate =  DateTime.Parse(x["published_at"].ToString());

                        Document d1 = new Document();
                        Author a1 = new Author();
                        a1.AuthorName = author;
                        dbContext.Authors.Add(a1);

                        d1.DocumentData = docData;
                        d1.author = a1;
                        d1.DocumentDate = pubDate;
                        d1.DocumentTitle = docTitle;
                        dbContext.Documents.Add(d1);

                    }
                }

                //dbContext.Documents.Add(parks);
                await dbContext.SaveChangesAsync();

               
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }

            return View("dataInsert");
    }

        public IActionResult Stories()
        {
            var stories=dbContext.Documents.Include(c=>c.cinfo).Include(c=>c.author).Select(c => c).ToList();
            ViewBag.storyBlockDetails = stories;
            return View("Stories");
        }

        public IActionResult About()
        {
            return View();
        }
 
        public IActionResult Write()
        {
           return View();
        }

        public IActionResult Update()
        {
            return View();
        }


        public async Task<ViewResult> postData(string writeTitle, string WriteData, string WriteName)
        {
            Document d = new Document();
            var author = dbContext.Authors.Where(c => c.AuthorName == WriteName).FirstOrDefault();
            if (author == null)
            {
                Author auth = new Author();
                auth.AuthorName = WriteName;
                dbContext.Authors.Add(auth);
                Document doc = new Document();
                doc.author = auth;
                doc.DocumentData = WriteData;
                doc.DocumentTitle = writeTitle;
                doc.DocumentDate = new DateTime();

                dbContext.Documents.Add(doc);

            }
            else
            {
                Document doc = new Document();
                doc.author = author;
                doc.DocumentData = WriteData;
                doc.DocumentTitle = writeTitle;

                dbContext.Documents.Add(doc);
            }
            await dbContext.SaveChangesAsync();
            ViewBag.addClasses = "displayModal";
            return View("Write");
        }

        public ViewResult updateDataForm(int id1)
        {
            var doc = dbContext.Documents.Include(c => c.author).Include(c => c.cinfo).Where(c => c.DocumentID == id1).First();
            ViewBag.id = id1;
            ViewBag.author = doc.author.AuthorName;
            ViewBag.documentDate = doc.DocumentDate;
            ViewBag.documentData = doc.DocumentData;
            ViewBag.documentTitle = doc.DocumentTitle;
            return View("Update");
        }
        // query to remove the blocks from database
        public async Task<ViewResult> DeleteBlock(int id1)
        {
            var doc = dbContext.Documents.Include(c => c.author).Include(c => c.cinfo).Where(c => c.DocumentID == id1).First();
            dbContext.Documents.Remove(doc);
            await dbContext.SaveChangesAsync();
            var storiesUpdated = dbContext.Documents.Include(c => c.cinfo).Include(c => c.author).Select(c => c).ToList();
            ViewBag.storyBlockDetails = storiesUpdated;
            return View("Stories");
        }
        public async Task<ViewResult> updateData(int id1, string updateName, string updateTitle, string textarea)
        {
            var doc = dbContext.Documents.Include(c => c.author).Include(c => c.cinfo).Where(c => c.DocumentID == id1).First();
            doc.DocumentData = textarea;
            doc.DocumentTitle = updateTitle;
            doc.author.AuthorName = updateName;
            dbContext.Documents.Update(doc);
            await dbContext.SaveChangesAsync();

            var storiesUpdated = dbContext.Documents.Include(c => c.cinfo).Include(c => c.author).Select(c => c).ToList();
            ViewBag.storyBlockDetails = storiesUpdated;

            return View("stories");
        }

        public async Task<ViewResult> DatabaseOperations()
    {
        //  CREATE operation on database for CRUD operations
            
            // Author details
            Author AuthorDetails = new Author();
            AuthorDetails.AuthorName = "Naveen";

            Author AuthorDetails1 = new Author();
            AuthorDetails1.AuthorName = "Akhil";

            Author AuthorDetails2 = new Author();
            AuthorDetails2.AuthorName = "Santhosh";


            Author AuthorDetails3 = new Author();
            AuthorDetails3.AuthorName = "Manideep";
            
            
            Author AuthorDetails2 = new Author();
            AuthorDetails2.AuthorName = "Santhosh";


            Author AuthorDetails3 = new Author();
            AuthorDetails3.AuthorName = "Manideep";

            //Doc
            DateTime d1 = new DateTime(2015, 3, 31);
            Document MyDocument = new Document();
            MyDocument.author = AuthorDetails;
            MyDocument.DocumentTitle = "OpenCV Library";
            MyDocument.ResearchField = "Machine Learning";
            MyDocument.DocumentData = "Machine Learning data";
            MyDocument.DocumentDate = d1;




            Document MyDocument5 = new Document();
            MyDocument5.author = AuthorDetails;
            MyDocument5.DocumentTitle = "sk learn";
            MyDocument5.ResearchField = "python";
            MyDocument5.DocumentData = "Machine Learning data";
            MyDocument5.DocumentDate = d1;

            Document MyDocument1 = new Document();
            MyDocument1.author = AuthorDetails1;
            MyDocument1.DocumentTitle = "matplot lib";
            MyDocument1.ResearchField = "visualization";
            MyDocument1.DocumentData = "Machine Learning data";
            MyDocument1.DocumentDate = d1;

            Document MyDocument2 = new Document();
            MyDocument2.author = AuthorDetails;
            MyDocument2.DocumentTitle = "React JS";
            MyDocument2.ResearchField = "Web development";
            MyDocument2.DocumentData = "Machine Learning data";
            MyDocument2.DocumentDate = d1;


            Document MyDocument3 = new Document();
            MyDocument3.author = AuthorDetails1;
            MyDocument3.DocumentTitle = "OpenCV Library67";
            MyDocument3.ResearchField = "Machine Learning";
            MyDocument3.DocumentData = "Machine Learning data";

            Document MyDocument4 = new Document();
            MyDocument4.author = AuthorDetails3;
            MyDocument4.DocumentTitle = "OpenCV Library787";
            MyDocument4.ResearchField = "Machine Learning";
            MyDocument4.DocumentData = "Machine Learning data";
            MyDocument4.DocumentDate = d1;


            //User

            User UserDetails = new User();
            UserDetails.UserFirstName = "Pranith";
            UserDetails.UserLastName = "Modem";

            User UserDetails1 = new User(); 
            UserDetails1.UserFirstName = "Avinash";
            UserDetails1.UserLastName = "Reddy";

            User UserDetails2 = new User();
            UserDetails2.UserFirstName = "Amrit";
            UserDetails2.UserLastName = "Nath";

            User UserDetails3 = new User();
            UserDetails3.UserFirstName = "Lokesh";
            UserDetails3.UserLastName = "Vazrala";



            //Comment INfo


            DateTime dt1 = new DateTime(2015, 12,31);
            Comment MyDocumentInfo = new Comment();
            MyDocumentInfo.CommentDate = dt1;
            MyDocumentInfo.doc = MyDocument1;
            MyDocumentInfo.user = UserDetails ;

            DateTime dt2 = new DateTime(2015, 2, 21);
            Comment MyDocumentInfo1 = new Comment();
            MyDocumentInfo1.CommentDate = dt2;
            MyDocumentInfo1.doc = MyDocument1;
            MyDocumentInfo1.user = UserDetails1;
            MyDocumentInfo1.CommentData = "excellent";

            DateTime dt3 = new DateTime(2015, 3, 31);
            Comment MyDocumentInfo2 = new Comment();
            MyDocumentInfo2.CommentDate = dt3;
            MyDocumentInfo2.doc = MyDocument1;
            MyDocumentInfo2.user = UserDetails3;
            MyDocumentInfo2.CommentData = "good";

            DateTime dt4 = new DateTime(2015, 1, 1);
            Comment MyDocumentInfo3 = new Comment();
            MyDocumentInfo3.CommentDate = dt4;
            MyDocumentInfo3.doc = MyDocument2;
            MyDocumentInfo3.user = UserDetails1;
            MyDocumentInfo3.CommentData = "good";

            DateTime dt5 = new DateTime(2015, 1, 21);
            Comment MyDocumentInfo4 = new Comment();
            MyDocumentInfo4.CommentDate = dt5;
            MyDocumentInfo4.doc = MyDocument2;
            MyDocumentInfo4.user = UserDetails3;
            MyDocumentInfo4.CommentData = "good";

            DateTime dt6 = new DateTime(2016, 12, 11);
            Comment MyDocumentInfo5 = new Comment();
            MyDocumentInfo5.CommentDate = dt6;
            MyDocumentInfo5.doc = MyDocument5;
            MyDocumentInfo5.user = UserDetails3;
            MyDocumentInfo5.CommentData = "super";

            DateTime dt7 = new DateTime(2015, 11, 15);
            Comment MyDocumentInfo6 = new Comment();
            MyDocumentInfo6.CommentDate = dt7;
            MyDocumentInfo6.doc = MyDocument3;
            MyDocumentInfo6.user = UserDetails2;
            MyDocumentInfo6.CommentData = "keka";


            // adding authors data
            dbContext.Authors.Add(AuthorDetails);
            dbContext.Authors.Add(AuthorDetails1);
            dbContext.Authors.Add(AuthorDetails2);
            dbContext.Authors.Add(AuthorDetails3);

            // adding documents data
            dbContext.Documents.Add(MyDocument);
            dbContext.Documents.Add(MyDocument1);
            dbContext.Documents.Add(MyDocument2);
            dbContext.Documents.Add(MyDocument3);
            dbContext.Documents.Add(MyDocument4);
            dbContext.Documents.Add(MyDocument5);

            // adding users data
            dbContext.Users.Add(UserDetails);
            dbContext.Users.Add(UserDetails1);
            dbContext.Users.Add(UserDetails2);
            dbContext.Users.Add(UserDetails3);

            // adding documentInfo data
            dbContext.Comments.Add(MyDocumentInfo);
            dbContext.Comments.Add(MyDocumentInfo1);
            dbContext.Comments.Add(MyDocumentInfo2);
            dbContext.Comments.Add(MyDocumentInfo3);
            dbContext.Comments.Add(MyDocumentInfo4);
            dbContext.Comments.Add(MyDocumentInfo5);
            dbContext.Comments.Add(MyDocumentInfo6);

            dbContext.SaveChanges();

            // Read operation
            //List<Author> publisherDetails = dbContext.Authors.Where(c => dbContext.Documents.Select(d=>d.author.AuthorID).ToList().Contains(c.AuthorID)).ToList();
                                     

      return View();
    }

    public ViewResult HomePageCard()
    {
           // var publisherDetails = dbContext.Authors.Where(c => dbContext.Documents.Select(d => d.author.AuthorID).Distinct().Contains(c.AuthorID)).Select(uid=>new { AuthorFirstName= uid.AuthorFirstName, AuthorLastName= uid.AuthorLastName }).ToList();
            Console.WriteLine("hello");
            var cardDetails = dbContext.Authors.Include(c=>c.document).Select(c => c).ToList();
            
            Console.WriteLine(cardDetails);
            ViewBag.CardDetails = cardDetails;

            string[] ChartLabels = new string[] { "January", "February", "March", "April", "May" };
            string[] ChartColors = new string[] { "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850" };

            int[] ChartData = new int[] { 34478, 33267, 3734, 3184,3433 };
            int[] ChartData1 = new int[] { 2248, 2567, 2274, 2878, 2033 };
            int[] ChartData2 = new int[] { 278, 267, 34, 78, 33 };
            int[] ChartData3 = new int[] { 947, 867, 734, 784, 633 };
            int[] ChartData4 = new int[] { 448, 567, 474, 678, 433 };
            int[] ChartData5 = new int[] { 278, 267, 34, 78, 33 };
            ViewBag.ChartType = "line";
            ViewBag.Labels = String.Join(",", ChartLabels.Select(d => "'" + d + "'"));
            ViewBag.Colors = String.Join(",", ChartColors.Select(d => "\"" + d + "\""));
            ViewBag.Data = String.Join(",", ChartData.Select(d => d));
            ViewBag.Data1 = String.Join(",", ChartData1.Select(d => d));
            ViewBag.Data2= String.Join(",", ChartData2.Select(d => d));
            ViewBag.Data3 = String.Join(",", ChartData3.Select(d => d));
            ViewBag.Data4 = String.Join(",", ChartData4.Select(d => d));
            ViewBag.Data5 = String.Join(",", ChartData5.Select(d => d));

            ViewBag.Title = "Crypto coin updates";
          

            return View("Index");
    }


        public ViewResult GetGraphData()
        {
          

                return View();
        }

        public async Task<ViewResult> updateComment(int id1, string newcomment)
    {

            var doc = dbContext.Documents.Include(c => c.author).Include(c => c.cinfo).Where(c => c.DocumentID == id1).First();
            Comment new1 = new Comment();
            new1.CommentData = newcomment;
            new1.CommentDate = new DateTime();
            new1.doc = doc;
            dbContext.Comments.Add(new1);
            await  dbContext.SaveChangesAsync();

            var storiesUpdated = dbContext.Documents.Include(c => c.cinfo).Include(c => c.author).Select(c => c).ToList();
            ViewBag.storyBlockDetails = storiesUpdated;
            return View("Stories");

    }
        /*  public ViewResult returnStories()
          {

              return View("Stories");
          }
        */
        public ViewResult Query2()
    {
           // var ex = dbContext.Documents.Where(p => p.ResearchField == "Machine Learning").ToList();
           // var publisherDetails = dbContext.Authors.Where(c => dbContext.Documents.Where(p=>p.ResearchField== "Machine Learning").Select(p=> p.author.AuthorID).Contains(c.AuthorID)).Select(uid => new { AuthorFirstName = uid.AuthorFirstName, AuthorLastName = uid.AuthorLastName }).ToList();
            Console.WriteLine("hello");

        return View();
    }

    public ViewResult Query3()
    {
           // var publisherDetails = dbContext.Documents.Where(c => dbContext.DocumentInfos.Where(p => p.DateDownloaded == "12/31/2015").Where(p => p.user.UserID==1).Select(p => p.doc.DocumentID).Contains(c.DocumentID)).Select(uid => new { DocumentTitle = uid.DocumentTitle, ResearchField = uid.ResearchField }).ToList();

            return View();
    }
    
    public ViewResult Query4()
        {
           // var publisherDetails = dbContext.Documents.GroupBy(q => q.ResearchField).OrderByDescending(gp => gp.Count())
          //                          .Take(2)
          //                          .Select(g => g.Key).ToList();

            return View();
    }

        public ViewResult Query5()
        {
           // var publisherDetails = dbContext.DocumentInfos.Include(s => s.doc).Include(s => s.user).GroupBy(q => q.doc.ResearchField).Select(q => new { id = q.Key, count=q.Count() }).OrderByDescending(g=>g.count).Take(2)
           //     .ToList();

            return View();
        }

       
         
    } 
}
