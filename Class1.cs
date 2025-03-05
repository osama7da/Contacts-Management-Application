using Microsoft.SqlServer.Server;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ContactsDataAccessLayer
{
    public class clsContactDataAccess
    {
        public static bool GetContactInfoByID(int ID, ref string FirstName, ref string LastName, ref string Email, ref string Phone,
            ref string Address, ref DateTime DateOfBirth, ref int CountryID, ref string ImagePath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from Contacts where ContactID=@ContactID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ContactID", ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;

                    FirstName = (string)reader["FirstName"];
                    LastName = (string)reader["LastName"];
                    Email = (string)reader["Email"];
                    Phone = (string)reader["Phone"];
                    Address = (string)reader["Address"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    CountryID = (int)reader["CountryID"];
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }



                }
                else
                {
                    isFound = false;
                    //Console.WriteLine("Contact Not Found");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                connection.Close();
            }

            return isFound;

        }



        public static int AddNewContact(string FirstName, string LastName, string Email, string Phone, string Address, DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "INSERT INTO [dbo].[Contacts]([FirstName],[LastName] ,[Email] ,[Phone] ,[Address],[DateOfBirth],[CountryID] ,[ImagePath])VALUES(@FirstName,@LastName,@Email,@Phone,@Address,@DateOfBirth,@CountryID,@ImagePath); select SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@DateOfBirth",DateOfBirth);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            command.Parameters.AddWithValue("@ImagePath", ImagePath);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    return insertedID;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  
            }
            
            return -1; 

        }
        public static bool UpdateContact(int ContactID, string FirstName, string LastName, string Email, string Phone, string Address, DateTime DateOfBirth, int CountryID, string ImagePath)
        {
            int rowsaffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "UPDATE [dbo].[Contacts] SET [FirstName]=@FirstName,[LastName] =@LastName,[Email] = @Email,[Phone] =@Phone,[Address] = @Address,[DateOfBirth] = @DateOfBirth,[CountryID] = @CountryID,[ImagePath] = @ImagePath WHERE ContactID=@ContactID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("ContactID", ContactID); 
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@CountryID", CountryID);
            if (ImagePath!="")
            {
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }else
            {
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            }

            try
            {
                connection.Open();
                rowsaffected = command.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return (rowsaffected > 0); 
            

        }

        public static bool DeleteContact(int ID)
        {
            int rowsAffected = 0;
            SqlConnection Connection =new  SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "DELETE FROM [dbo].[Contacts] WHERE ContactID=@ContactID"; 
            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("ContactID", ID); 

            try
            {
                Connection.Open();
                rowsAffected= command.ExecuteNonQuery(); 

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }finally
            {
                Connection.Close();
            }
            return (rowsAffected > 0); 

        }

        public static DataTable GetAllContact()
        {
            DataTable datatable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select * from Contacts";
            SqlCommand command = new SqlCommand(query, Connection);

            try
            {
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    datatable.Load(reader);
                }
                reader.Close(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                
                Connection.Close();
            }
           return datatable;    
            
        }
        public static bool IsContactExist(int ContactID)
        {
            bool IsFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "Select found =1 from Contacts where ContactID=@ContactID"; 
            

            SqlCommand command = new SqlCommand(query, connection); 
            command.Parameters.AddWithValue("ContactID", ContactID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
              IsFound = reader.HasRows;
                reader.Close(); 
            }catch(Exception ex)
            {
                Console.WriteLine (ex.Message.ToString());
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }

        static class clsDataAccessSettings
        {
            public static string ConnectionString = "Server=.;Database=Contacts;User Id=sa;Password=sa123456;";
        }
    }
    public class clsCountriesDataAccess
    {
        public static bool GetCountryByID(int CountryID,ref string  CountryName)
        { 
            bool isFound=false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select * from Countries where CountryID=@CountryID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID); 

            try
            {
                connection.Open();  
                SqlDataReader reader = command.ExecuteReader(); 
                if (reader.Read())
                {
                    isFound = true;
                    CountryID = (int)reader["CountryID"];
                    CountryName = (string)reader["CountryName"]; 
                    
                }
                reader.Close(); 
            }catch (Exception ex) { Console.WriteLine (ex.Message); }   
            finally
            {
                connection.Close();
            }
            return (isFound); 


        }

        public static int AddNewCountry(string CountryName)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "INSERT INTO [dbo].[Countries]([CountryName])VALUES(@CountryName) ; select SCOPE_IDENTITY() ; ";

            SqlCommand command = new SqlCommand (query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName); 

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int idenity))
                {
                    return idenity;
                }
                else return -1; 
            }catch(Exception ex)
            {
                Console.WriteLine (ex.Message);
            }finally
            {
                connection.Close();
            } 
            return -1;

        }

        public static bool UpdateCountry(int ID , string CountryName)
        {
            int rowaffected = 0; 
            SqlConnection connection = new SqlConnection( clsDataAccessSettings.ConnectionString);
            string query = "UPDATE [dbo].[Countries] SET [CountryName] = @CountryName WHERE CountryID=@CountryID"; 
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("CountryID", ID); 
            command.Parameters.AddWithValue ("CountryName", CountryName); 

            try
            {
                connection.Open();
             rowaffected= command.ExecuteNonQuery();
               
               
            }catch(Exception ex)
            {
                Console.WriteLine (ex.Message);
            }finally
            {
                connection.Close();
            }
            return (rowaffected>0);
        }

        public static bool DeleteCountry(int ID)
        { 
            int rowaffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "DELETE FROM [dbo].[Countries] WHERE CountryID=@CountryID"; 
            SqlCommand commannd = new SqlCommand(query, connection);
            commannd.Parameters.AddWithValue("CountryID", ID);
            

            try
            {
                connection.Open();
                rowaffected = commannd.ExecuteNonQuery();
                

            } catch (Exception ex) { Console.WriteLine(ex.Message); } 
            finally
            {
                connection.Close();
            } 
            return (rowaffected>0);

        }

        public static bool FindCountryByName(string CountryName)
        { 
            SqlConnection connection = new SqlConnection(   clsDataAccessSettings.ConnectionString);
            string query = "select found=1 from Countries where CountryName=@CountryName"; 
            SqlCommand commannd = new SqlCommand( query, connection);
            commannd.Parameters.AddWithValue("CountryName", CountryName);

            try
            {
                connection.Open();
                SqlDataReader reader = commannd.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); } 
            finally { connection.Close(); }
            return false;

        }

        public static DataTable GetAllCountries()
        {
            DataTable datatable = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select CountryName from Countries";
            SqlCommand commannd = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = commannd.ExecuteReader();
                if (reader.HasRows)
                {
                    datatable.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { connection.Close(); }
            return datatable;
        }
        public static bool GetCountryInfoByName(string CountryName, ref int ID)
                                               
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;

                    ID = (int)reader["CountryID"];

                }
                else
                {
                
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
             
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        static class clsDataAccessSettings
        {
            public static string ConnectionString = "Server=.;Database=Contacts;User Id=sa;Password=sa123456;";
        }
    }

}