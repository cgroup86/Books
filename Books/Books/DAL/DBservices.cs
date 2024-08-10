using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Books.BL;
using System.Data.Common;

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
        cmd.Parameters.AddWithValue("@key", author.Key);

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
        cmd.Parameters.AddWithValue("@publisher", book.Publisher);
        cmd.Parameters.AddWithValue("@publishedDate", book.PublishedDate);
        cmd.Parameters.AddWithValue("@description", book.Description);
        cmd.Parameters.AddWithValue("@pageNum", book.PageCount);
        cmd.Parameters.AddWithValue("@averageRating", book.AverageRating);
        cmd.Parameters.AddWithValue("@ratingsCount", book.RatingsCount);
        cmd.Parameters.AddWithValue("@smallThumbnailUrl", book.SmallThumbnailUrl);
        cmd.Parameters.AddWithValue("@thumbnailUrl", book.ThumbnailUrl);
        cmd.Parameters.AddWithValue("@lang", book.Language);
        cmd.Parameters.AddWithValue("@previewLink", book.PreviewLink);
        cmd.Parameters.AddWithValue("@infoLink", book.InfoLink);
        cmd.Parameters.AddWithValue("@canonicalVolumeLink", book.CanonicalVolumeLink);
        cmd.Parameters.AddWithValue("@isEbook", book.IsEbook);
        cmd.Parameters.AddWithValue("@embeddable", book.Embeddable);
        cmd.Parameters.AddWithValue("@epubIsAvailable", book.EpubIsAvailable);
        cmd.Parameters.AddWithValue("@epubDownloadLink", book.EpubDownloadLink);
        cmd.Parameters.AddWithValue("@pdfIsAvailable", book.PdfIsAvailable);
        cmd.Parameters.AddWithValue("@pdfDownloadLink", book.PdfDownloadLink);
        cmd.Parameters.AddWithValue("@webReaderLink", book.WebReaderLink);
        cmd.Parameters.AddWithValue("@textReading", book.TextReading);
        cmd.Parameters.AddWithValue("@photoReading", book.PhotoReading);
        cmd.Parameters.AddWithValue("@googleBooksId", book.GoogleBooksId);
        cmd.Parameters.AddWithValue("@etag", book.Etag);
        cmd.Parameters.AddWithValue("@selfLink", book.SelfLink);



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
    // This method Get Author from the isntructor table 
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
                    Name = dataReader["name"].ToString(),
                    TopWork = dataReader["topwork"].ToString(),
                    WorkCount = Convert.ToInt32(dataReader["workCount"]),
                    Key = dataReader["key"].ToString()
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

}