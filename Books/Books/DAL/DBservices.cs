using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Books.BL;
using System.Data.Common;
using System.Net;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{

    public DBservices() { }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }


    //---------------------------------------------------------------------------------
    // this method to read the Users from DB table
    //---------------------------------------------------------------------------------

    public List<User> ReadUsers()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureReadUsers("SP_ReadUsers", con); // create the command

        List<User> users = new List<User>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                User user = new User()
                {
                    Id = Convert.ToInt32(dataReader["id"]),
                    Name = dataReader["name"].ToString(),
                    Email = dataReader["email"].ToString(),
                    Password = dataReader["password"].ToString(),
                    IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]),
                    IsActive = Convert.ToBoolean(dataReader["isActive"])
                };
                users.Add(user);
            }
            return users;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadUsers(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method register a new user
    //--------------------------------------------------------------------------------------------------
    public void UserRegister(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureUserRegister("SP_UserRegister", con, user); // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dataReader.Read())
            {
                int result = Convert.ToInt32(dataReader["Result"]);

                if (result == 0)
                {
                    throw new ArgumentException("User with this email already exists");
                }
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUserRegister(String spName, SqlConnection con, User user)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@name", user.Name);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@password", user.Password);

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method logs the user in
    //--------------------------------------------------------------------------------------------------
    public bool UserLogin(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureLoginUsers("SP_UserLogin", con, user); // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read() && Convert.ToInt32(dataReader["UserId"]) >= 0)
            {
                user.Id = Convert.ToInt32(dataReader["UserId"]);
                user.Name = dataReader["name"].ToString();
                user.IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]);
                user.IsActive = Convert.ToBoolean(dataReader["isActive"]);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureLoginUsers(String spName, SqlConnection con, User user)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@Password", user.Password);

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method insert author to the author table 
    //--------------------------------------------------------------------------------------------------
    public int insertAuthor(Author author)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureInsertAuthor("SP_InsertAuthor", con, author);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertAuthor(String spName, SqlConnection con, Author author)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@name", author.Name);
        cmd.Parameters.AddWithValue("@topWork", author.TopWork);
        cmd.Parameters.AddWithValue("@workCount", author.WorkCount);

        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method insert Category to the category table 
    //--------------------------------------------------------------------------------------------------
    public int insertCategory(Category category)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureInsertCategory("SP_InsertCategory", con, category);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertCategory(String spName, SqlConnection con, Category category)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@name", category.Name);


        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method insert book to the books table 
    //--------------------------------------------------------------------------------------------------
    public int InsertBook(Book book)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureInsertBook("SP_InsertBook", con, book);             // create the command

        try
        {
            object result = cmd.ExecuteScalar();
            int bookId = Convert.ToInt32(result);
            return bookId;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertBook(String spName, SqlConnection con, Book book)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@BookTitle", book.Title);
        cmd.Parameters.AddWithValue("@price", book.Price);
        cmd.Parameters.AddWithValue("@description", book.Description);
        cmd.Parameters.AddWithValue("@pageNum", book.PageCount);
        cmd.Parameters.AddWithValue("@smallThumbnailUrl", book.SmallThumbnailUrl);
        cmd.Parameters.AddWithValue("@lang", book.Language);
        cmd.Parameters.AddWithValue("@previewLink", book.PreviewLink);
        cmd.Parameters.AddWithValue("@isEbook", book.IsEbook);
        cmd.Parameters.AddWithValue("@embeddable", book.Embeddable);
        cmd.Parameters.AddWithValue("@webReaderLink", book.WebReaderLink);
        cmd.Parameters.AddWithValue("@googleBooksId", book.GoogleBooksId);
        cmd.Parameters.AddWithValue("@isActive", book.IsActive);
        cmd.Parameters.AddWithValue("@isAvailable", book.IsAvailable);
        cmd.Parameters.AddWithValue("@numOfPrints", book.NumOfPrints);

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method insert category to the book categories 
    //--------------------------------------------------------------------------------------------------
    public void InsertBookCategory(string category, int bookId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureInsertBookCategory("SP_InsertToBookCategory", con, category, bookId);   // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertBookCategory(String spName, SqlConnection con, string category, int bookId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text


        cmd.Parameters.AddWithValue("@category", category);
        cmd.Parameters.AddWithValue("@bookId", bookId);

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method insert author to the book authors
    //--------------------------------------------------------------------------------------------------
    public void InsertBookAuthor(string authorName, int bookId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureInsertBookAuthor("SP_InsertBookAuthor", con, authorName, bookId);   // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureInsertBookAuthor(String spName, SqlConnection con, string authorName, int bookId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text


        cmd.Parameters.AddWithValue("@authorName", authorName);
        cmd.Parameters.AddWithValue("@bookId", bookId);

        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method Get Author from the Author table 
    //--------------------------------------------------------------------------------------------------
    public List<Author> readAuthors()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureReadAuthors("SP_ReadAuthors", con); // create the command

        List<Author> authors = new List<Author>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Author author = new Author
                {
                    Name = dataReader["AuthorName"].ToString(),
                    TopWork = dataReader["topwork"].ToString(),
                    WorkCount = Convert.ToInt32(dataReader["workCount"]),
                    Image = dataReader["images"].ToString(),
                    Description = dataReader["Description"].ToString()
                };
                if (author.Name != null && author.Name != "")
                {
                    authors.Add(author);
                }
            }
            return authors;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadAuthors(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method insert images to the Author table 
    //--------------------------------------------------------------------------------------------------
    public int insertImagesOfAuthors(string name, string image, string description)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureIinsertImagesOfAuthors("SP_InsertImages", con, name, image, description);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureIinsertImagesOfAuthors(String spName, SqlConnection con, string name, string image, string description)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@image", image);
        cmd.Parameters.AddWithValue("@description", description);

        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method gets paged books from the data base
    //--------------------------------------------------------------------------------------------------
    public List<Book> GetPagedBooks(bool isEbook, int pageNumber, int pageSize, out int totalRecords, bool fetchTotalCount)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureGetPagedBooks("SP_GetPagedBooks", con, isEbook, pageNumber, pageSize, fetchTotalCount); // create the command

        List<Book> books = new List<Book>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {

                Book book = new Book
                {
                    Id = Convert.ToInt32(dataReader["BookId"]),
                    Title = dataReader["BookTitle"].ToString(),
                    Price = Convert.ToInt32(dataReader["price"]),
                    SmallThumbnailUrl = dataReader["smallThumbnailUrl"].ToString(),
                    PreviewLink = dataReader["previewLink"].ToString(),
                    IsEbook = bool.Parse(dataReader["isEbook"].ToString()),
                    WebReaderLink = dataReader["webReaderLink"].ToString(),
                    IsAvailable = bool.Parse(dataReader["isAvailable"].ToString()),
                };
                books.Add(book);
            }
            // TotalRecords = Convert.ToInt32(dataReader["TotalRecords"]),
            // dataReader["infoLink"]?.ToString(),
            if (dataReader.NextResult() && dataReader.Read())
            {
                // Check if the TotalRecords value is DBNull before converting
                if (dataReader["TotalRecords"] != DBNull.Value)
                {
                    totalRecords = Convert.ToInt32(dataReader["TotalRecords"]);
                }
                else
                {
                    totalRecords = -1; // Set to 0 or another default value if NULL
                }
            }
            else
            {
                totalRecords = -1;
            }

            return books;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetPagedBooks(String spName, SqlConnection con, bool isEbook, int pageNumber, int pageSize, bool fetchTotalCount)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@IsEbook", isEbook);
        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);
        cmd.Parameters.AddWithValue("@FetchTotalCount", fetchTotalCount);
        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method Get Author from the Author table 
    //--------------------------------------------------------------------------------------------------
    public List<string> GetAuthorsForBook(int bookId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureGetAuthorsForBook("SP_GetAuthorsForBook", con, bookId); // create the command

        List<string> authors = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                authors.Add(dataReader["AuthorName"].ToString());
            }
            return authors;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetAuthorsForBook(String spName, SqlConnection con, int bookId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@BookId", bookId);

        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method adds a book for a user in the  status want to read;
    //--------------------------------------------------------------------------------------------------
    public bool AddToLibrary(PersonalLibrary personalLibrary)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureAddToLibrary("SP_InsertToPersonalLibrary", con, personalLibrary);   // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dataReader.Read())
            {
                int result = Convert.ToInt32(dataReader["Result"]);
                return result > 0;
            }
            return false;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureAddToLibrary(String spName, SqlConnection con, PersonalLibrary personalLibrary)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text


        cmd.Parameters.AddWithValue("@UserId", personalLibrary.UserId);
        cmd.Parameters.AddWithValue("@BookId", personalLibrary.BookId);
        cmd.Parameters.AddWithValue("@Status", personalLibrary.Status);
        cmd.Parameters.AddWithValue("@IsPurchased", personalLibrary.IsPurchased);

        return cmd;
    }



    ////--------------------------------------------------------------------------------------------------
    //// This method to get 10 Authors per page  
    ////--------------------------------------------------------------------------------------------------

    public List<Author> readAuthorsByPage(int pageNumber, int pageSize)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureReadAuthors("GetAuthorsByTypePaginated", con, pageNumber, pageSize); // create the command

        List<Author> authors = new List<Author>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Author author = new Author()
                {
                    Name = dataReader["AuthorName"].ToString(),
                    TopWork = dataReader["topwork"].ToString(),
                    WorkCount = Convert.ToInt32(dataReader["workCount"]),
                    Image = dataReader["images"].ToString(),
                    Description = dataReader["Description"].ToString()
                };
                authors.Add(author);
            }
            return authors;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for reading authors
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadAuthors(string spName, SqlConnection con, int pageNumber, int pageSize)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;          // stored procedure name
        cmd.CommandTimeout = 10;           // set timeout to 10 seconds
        cmd.CommandType = CommandType.StoredProcedure; // specify that this is a stored procedure

        // Add parameters for pagination
        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);

        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method gets questions num 1
    //--------------------------------------------------------------------------------------------------

    public Question GetQuestion1()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        cmd = new SqlCommand("spGenerateQuestion1", con);
        cmd.CommandType = CommandType.StoredProcedure;

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            dataReader.Read();
            string questionText = dataReader["Question"].ToString();
            string correctAnswer = dataReader["CorrectAnswer"].ToString();

            List<object> wrongAnswers = new List<object>();

            if (dataReader.NextResult())
            {
                while (dataReader.Read())
                {
                    wrongAnswers.Add(dataReader["WrongAnswers"].ToString());
                }
            }

            List<object> allAnswers = new List<object>(wrongAnswers)
            {
                correctAnswer
            };

            Random rnd = new Random();
            allAnswers = allAnswers.OrderBy(x => rnd.Next()).ToList();

            var question = new Question()
            {
                QuestionText = questionText,
                Answers = allAnswers,
                CorrectAnswer = correctAnswer
            };

            return question;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    //--------------------------------------------------------------------------------------------------
    // This method gets questions num 2
    //--------------------------------------------------------------------------------------------------
    public Question GetQuestion2()
    {
        SqlConnection con = null;
        SqlCommand cmd = null;

        try
        {
            con = connect("myProjDB");
            cmd = new SqlCommand("spGenerateQuestion2", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            dataReader.Read();
            string questionText = dataReader["Question"].ToString();
            string correctAnswerTitle = dataReader["CorrectAnswerTitle"].ToString();
            string correctAnswerImage = dataReader["CorrectAnswerImage"].ToString();

            List<object> wrongAnswers = new List<object>();

            if (dataReader.NextResult())
            {
                while (dataReader.Read())
                {
                    wrongAnswers.Add(new
                    {
                        WrongAnswerTitle = dataReader["WrongAnswersTitle"].ToString(),
                        WrongAnswerImage = dataReader["WrongAnswersImage"].ToString()
                    });
                }
            }

            var correctAnswer = new
            {
                WrongAnswerTitle = correctAnswerTitle,
                WrongAnswerImage = correctAnswerImage
            };

            List<object> allAnswers = new List<object>(wrongAnswers)
            {
                correctAnswer
            };

            Random rnd = new Random();
            allAnswers = allAnswers.OrderBy(x => rnd.Next()).ToList();

            var question = new Question()
            {
                QuestionText = questionText,
                Answers = allAnswers,
                CorrectAnswer = correctAnswerTitle,
                CorrectAnswerImage = correctAnswerImage
            };

            return question;
        }
        catch (Exception ex)
        {
            throw; // Preserve stack trace
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method gets questions num 3
    //--------------------------------------------------------------------------------------------------

    public Question GetQuestion3()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        cmd = new SqlCommand("spGenerateQuestion3", con);
        cmd.CommandType = CommandType.StoredProcedure;

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            dataReader.Read();
            string questionText = dataReader["Question"].ToString();
            string correctAnswer = dataReader["CorrectAnswer"].ToString();

            List<object> wrongAnswers = new List<object>();

            if (dataReader.NextResult())
            {
                while (dataReader.Read())
                {
                    wrongAnswers.Add(dataReader["WrongAnswers"].ToString());
                }
            }

            List<object> allAnswers = new List<object>(wrongAnswers)
            {
                correctAnswer
            };

            Random rnd = new Random();
            allAnswers = allAnswers.OrderBy(x => rnd.Next()).ToList();

            var question = new Question()
            {
                QuestionText = questionText,
                Answers = allAnswers,
                CorrectAnswer = correctAnswer
            };

            return question;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method gets questions num 4
    //--------------------------------------------------------------------------------------------------

    public Question GetQuestion4()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw ex;
        }

        cmd = new SqlCommand("spGenerateQuestion4", con);
        cmd.CommandType = CommandType.StoredProcedure;

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            dataReader.Read();
            string questionText = dataReader["Question"].ToString();
            string correctAnswer = dataReader["CorrectAnswer"].ToString();

            List<object> wrongAnswers = new List<object>();

            if (dataReader.NextResult())
            {
                while (dataReader.Read())
                {
                    wrongAnswers.Add(dataReader["WrongAnswers"].ToString());
                }
            }

            List<object> allAnswers = new List<object>(wrongAnswers)
            {
                correctAnswer
            };

            Random rnd = new Random();
            allAnswers = allAnswers.OrderBy(x => rnd.Next()).ToList();

            var question = new Question()
            {
                QuestionText = questionText,
                Answers = allAnswers,
                CorrectAnswer = correctAnswer
            };

            return question;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method gets questions num 5
    //--------------------------------------------------------------------------------------------------
    public Question GetQuestion5()
    {
        SqlConnection con = null;
        SqlCommand cmd = null;

        try
        {
            con = connect("myProjDB");
            cmd = new SqlCommand("spGenerateQuestion5", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            dataReader.Read();
            string questionText = dataReader["Question"].ToString();
            string correctAnswerTitle = dataReader["CorrectAnswerTitle"].ToString();
            string correctAnswerImage = dataReader["CorrectAnswerImage"].ToString();

            List<object> wrongAnswers = new List<object>();

            if (dataReader.NextResult())
            {
                while (dataReader.Read())
                {
                    wrongAnswers.Add(new
                    {
                        WrongAnswerTitle = dataReader["WrongAnswersTitle"].ToString(),
                        WrongAnswerImage = dataReader["WrongAnswersImage"].ToString()
                    });
                }
            }

            var correctAnswer = new
            {
                WrongAnswerTitle = correctAnswerTitle,
                WrongAnswerImage = correctAnswerImage
            };

            List<object> allAnswers = new List<object>(wrongAnswers)
            {
                correctAnswer
            };

            Random rnd = new Random();
            allAnswers = allAnswers.OrderBy(x => rnd.Next()).ToList();

            var question = new Question()
            {
                QuestionText = questionText,
                Answers = allAnswers,
                CorrectAnswer = correctAnswerTitle,
                CorrectAnswerImage = correctAnswerImage
            };

            return question;
        }
        catch (Exception ex)
        {
            throw; // Preserve stack trace
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method is changing the attribute of the variable isActive
    //--------------------------------------------------------------------------------------------------
    public void UpdateBookStatus(int userId, int bookId, bool newStatus)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureUpdateBookStatus("SP_UpdateBookStatus", con, userId, bookId, newStatus);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dataReader.Read())
            {
                int result = Convert.ToInt32(dataReader["Result"]);

                if (result == 0)
                {
                    throw new ArgumentException("No book found with the specified BookId.");
                }

            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUpdateBookStatus(String spName, SqlConnection con, int userId, int bookId, bool newStatus)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@UserId", userId);
        cmd.Parameters.AddWithValue("@BookId", bookId);
        cmd.Parameters.AddWithValue("@NewStatus", newStatus); 
        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method Get BooksToRead
    //--------------------------------------------------------------------------------------------------
    public List<object> GetBooksToRead(int userId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureGetBooksToRead("SP_GetBooksToRead", con, userId); // create the command

        List<object> listObj = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                listObj.Add(new
                {
                    bookId = Convert.ToInt32(dataReader["BookId"]),
                    bookTitle = dataReader["BookTitle"].ToString(),
                    smallThumbnailUrl = dataReader["smallThumbnailUrl"].ToString(),
                    isEbook = bool.Parse(dataReader["isEbook"].ToString()),
                    webReaderLink = dataReader["webReaderLink"].ToString(),
                    embeddable = bool.Parse(dataReader["embeddable"].ToString()),
                    googleBooksId = dataReader["googleBooksId"].ToString(),
                    status = bool.Parse(dataReader["status"].ToString()),
                    IsPurchased = bool.Parse(dataReader["IsPurchased"].ToString()),
                    isActive = bool.Parse(dataReader["isActive"].ToString()),
                });               
            }
            return listObj;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetBooksToRead(String spName, SqlConnection con, int userId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@UserId", userId);

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method Get BooksRead
    //--------------------------------------------------------------------------------------------------
    public List<object> GetBooksRead(int userId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureGetBooksRead("SP_GetBooksRead", con, userId); // create the command

        List<object> listObj = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                listObj.Add(new
                {
                    bookId = Convert.ToInt32(dataReader["BookId"]),
                    bookTitle = dataReader["BookTitle"].ToString(),
                    smallThumbnailUrl = dataReader["smallThumbnailUrl"].ToString(),
                    isEbook = bool.Parse(dataReader["isEbook"].ToString()),
                    webReaderLink = dataReader["webReaderLink"].ToString(),
                    embeddable = bool.Parse(dataReader["embeddable"].ToString()),
                    googleBooksId = dataReader["googleBooksId"].ToString(),
                    status = bool.Parse(dataReader["status"].ToString()),
                    IsPurchased = bool.Parse(dataReader["IsPurchased"].ToString()),
                    isActive = bool.Parse(dataReader["isActive"].ToString()),
                });
            }
            return listObj;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetBooksRead(String spName, SqlConnection con, int userId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@UserId", userId);

        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method Get BooksPurchased
    //--------------------------------------------------------------------------------------------------
    public List<object> GetBooksPurchased(int userId)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureGetBooksPurchased("SP_GetBooksPurchased", con, userId); // create the command

        List<object> listObj = new List<object>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                listObj.Add(new
                {
                    bookId = Convert.ToInt32(dataReader["BookId"]),
                    bookTitle = dataReader["BookTitle"].ToString(),
                    smallThumbnailUrl = dataReader["smallThumbnailUrl"].ToString(),
                    isEbook = bool.Parse(dataReader["isEbook"].ToString()),
                    webReaderLink = dataReader["webReaderLink"].ToString(),
                    embeddable = bool.Parse(dataReader["embeddable"].ToString()),
                    googleBooksId = dataReader["googleBooksId"].ToString(),
                    status = bool.Parse(dataReader["status"].ToString()),
                    IsPurchased = bool.Parse(dataReader["IsPurchased"].ToString()),
                    isActive = bool.Parse(dataReader["isActive"].ToString()),
                });
            }
            return listObj;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureGetBooksPurchased(String spName, SqlConnection con, int userId)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@UserId", userId);

        return cmd;
    }

    ////--------------------------------------------------------------------------------------------------
    //// This method to get books by author name 
    ////--------------------------------------------------------------------------------------------------

    public List<Book> readBooksByAuthorName(string authorName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureReadBooksByAuthorName("GetBooksByAuthorName", con, authorName); // create the command

        List<Book> books = new List<Book>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Book book = new Book
                {
                    Id = Convert.ToInt32(dataReader["BookId"]),
                    Title = dataReader["BookTitle"].ToString(),
                    Price = Convert.ToInt32(dataReader["price"]),
                    PageCount = Convert.ToInt32(dataReader["pageNum"]),
                    SmallThumbnailUrl = dataReader["smallThumbnailUrl"].ToString(),
                    PreviewLink = dataReader["previewLink"].ToString(),
                    IsEbook = bool.Parse(dataReader["isEbook"].ToString()),
                    WebReaderLink = dataReader["webReaderLink"].ToString(),
                };

                books.Add(book);
            }

            return books;

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for reading authors
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadBooksByAuthorName(string spName, SqlConnection con, string authorName)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object
        cmd.CommandText = spName;          // stored procedure name
        cmd.CommandTimeout = 10;           // set timeout to 10 seconds
        cmd.CommandType = CommandType.StoredProcedure; // specify that this is a stored procedure

        // Add parameters for pagination
        cmd.Parameters.AddWithValue("@authorName", authorName);

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method is changing the attribute of the variable isActive
    //--------------------------------------------------------------------------------------------------
    public int SubmitScore(UserScore userScore)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureSubmitScore("SP_UpsertQuizScore", con, userScore);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureSubmitScore(String spName, SqlConnection con, UserScore userScore)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@UserId", userScore.UserId);
        cmd.Parameters.AddWithValue("@Score", userScore.Score);
        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method gets questions num 5
    //--------------------------------------------------------------------------------------------------
    public UserScore GetUserAndTopScores(int userId)
    {
        SqlConnection con = null;
        SqlCommand cmd = null;

        try
        {
            con = connect("myProjDB");
            cmd = new SqlCommand("SP_GetUserAndTopScores", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            dataReader.Read();
            int userID = Convert.ToInt32(dataReader["UserId"]);
            string userName = dataReader["UserName"].ToString();
            int score = Convert.ToInt32(dataReader["Score"]);

            List<object> topUserScores = new List<object>();

            if (dataReader.NextResult())
            {
                while (dataReader.Read())
                {
                    topUserScores.Add(new
                    {
                        UserName = dataReader["UserName"].ToString(),
                        Score = dataReader["Score"].ToString()
                    });
                }
            }

            var userScore = new UserScore()
            {
                UserId = userID,
                Score = score,
                TopUserScores = topUserScores
            };

            return userScore;
        }
        catch (Exception ex)
        {
            throw; // Preserve stack trace
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    ////--------------------------------------------------------------------------------------------------
    //// This method to get all books by page
    ////--------------------------------------------------------------------------------------------------

    public List<Book> readBooksByPage(int pageNumber, int pageSize)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureReadBooks("GetBooksByPaginated", con, pageNumber, pageSize); // create the command

        List<Book> books = new List<Book>();
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Book book = new Book()
                {
                    Id = Convert.ToInt32(dataReader["BookId"]),
                    Title = dataReader["BookTitle"].ToString(),
                    Price = Convert.ToInt32(dataReader["price"]),
                    Description = dataReader["description"].ToString(),
                    SmallThumbnailUrl = dataReader["smallThumbnailUrl"].ToString(),
                    Language = dataReader["lang"].ToString(),
                    IsEbook = bool.Parse(dataReader["isEbook"].ToString()),
                    IsActive = bool.Parse(dataReader["isActive"].ToString()),
                    IsAvailable = bool.Parse(dataReader["isAvailable"].ToString()),
                    NumOfPrints = Convert.ToInt32(dataReader["numOfprints"]),
                };

                books.Add(book);
            }
            return books;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure for reading authors
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureReadBooks(string spName, SqlConnection con, int pageNumber, int pageSize)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Connection = con;
        cmd.CommandText = spName;
        cmd.CommandTimeout = 10;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);
        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method Upadating the Books values
    //--------------------------------------------------------------------------------------------------
    public int UPdateBookValues(int bookId, bool isActive, int newPrice, int numberOfPrints)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureUPdateBookValues("UpdateBooksValues", con, bookId, isActive, newPrice, numberOfPrints); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUPdateBookValues(String spName, SqlConnection con, int bookId, bool isActive, int newPrice, int numberOfPrints)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@bookId", bookId);
        cmd.Parameters.AddWithValue("@active", isActive);
        cmd.Parameters.AddWithValue("@newPrice", newPrice);
        cmd.Parameters.AddWithValue("@numberOfPrints", numberOfPrints);

        return cmd;
    }



    //--------------------------------------------------------------------------------------------------
    // This method Upadating the User is Active value
    //--------------------------------------------------------------------------------------------------
    public int UPdateUserValue(int userId, bool isActive)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedureUPdateUserValue("UpdateUserIsActiveValue", con, userId, isActive); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedureUPdateUserValue(String spName, SqlConnection con, int userId, bool isActive)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@userId", userId);
        cmd.Parameters.AddWithValue("@active", isActive);

        return cmd;
    }
}