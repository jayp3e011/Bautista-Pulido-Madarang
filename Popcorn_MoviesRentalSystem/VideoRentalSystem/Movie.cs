﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoRentalSystem
{
    class Movie
    {
        public Movie()
        {

        }
        private string yearRelease;
        private string cast;
        private string language;
        private int rating;
        private string genre;
        private string name;
        private string movieID;
        private string imgPath;
        public string Movie_ID
        {
            get { return movieID; }
            set { movieID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }

        public int Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        public string Cast
        {
            get { return cast; }
            set { cast = value; }
        }

        public string Year_Release
        {
            get { return yearRelease; }
            set { yearRelease = value; }
        }

        public string ImgPath
        {
            get { return imgPath; }
            set { imgPath = value; }
        }

        public void generateID()
        {
            string id = "";
            string[] n = name.Split(' ');
            foreach (string s in n)
            {
                id += s.Substring(0, 1);
            }
            Movie_ID = yearRelease + "_" + id;
        }

        public void insert()
        {
            string sql = "INSERT INTO tbl_movies (movie_id, movie_title, movie_genre, movie_rating, movie_language, movie_casts, movie_yearReleased) VALUES('" + this.movieID + "', '" + this.name + "', '" + this.genre + "', '" + this.rating + "', '" + this.language + "', '" + this.cast + "', '" + this.yearRelease + "')";
            connectDB conn = new connectDB();
            conn.connect();
            conn.query(sql);

        }

        public void update()
        {
            string sql = "UPDATE tbl_movies SET movie_title = " + this.name + ", movie_genre = " + this.genre + ", movie_rating = " + this.rating + ", movie_language = " + this.language + ", movie_casts = " + this.cast + ", movie_yearReleased = " + this.yearRelease + " WHERE movie_id = " + this.movieID + "";
            connectDB conn = new connectDB();
            conn.connect();
            conn.query(sql);
        }

        public void delete()
        {
            string sql = "DELETE FROM tbl_movies WHERE movie_id = " + this.movieID + "";
            connectDB conn = new connectDB();
            conn.connect();
            conn.query(sql);
        }

        public void select()
        {
            string sql = "SELECT movie_id, movie_title, movie_genre, movie_rating, movie_language, movie_casts, movie_yearReleased FROM tbl_movies";
            connectDB conn = new connectDB();
            conn.connect();
            conn.query(sql);
        }
        public void spTest(string statementType)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["videoRental"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("movieQuery", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@movie_id", this.movieID));
                    cmd.Parameters.Add(new SqlParameter("@movie_title", this.name));
                    cmd.Parameters.Add(new SqlParameter("@movie_genre", this.genre));
                    cmd.Parameters.Add(new SqlParameter("@movie_rating", this.rating));
                    cmd.Parameters.Add(new SqlParameter("@movie_language", this.language));
                    cmd.Parameters.Add(new SqlParameter("@movie_casts", this.cast));
                    cmd.Parameters.Add(new SqlParameter("@movie_yearReleased", this.yearRelease));
                    cmd.Parameters.Add(new SqlParameter("@movie_image", this.imgPath));
                    cmd.Parameters.Add(new SqlParameter("@StatementType", statementType));
                    con.Open();
                    MessageBox.Show(cmd.ExecuteNonQuery().ToString());

                }
            }
        }
        public string getMovieImg()
        {
            SqlDataReader reader;
            string path = "";
            //MemoryStream ms=null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["videoRental"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("movieQuery", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@movie_id", this.movieID));
                    cmd.Parameters.Add(new SqlParameter("@StatementType", "GetMovieImg"));
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        path = reader["movie_image"].ToString();
                        //Byte[] byteBLOBData = new Byte[0];
                        //byteBLOBData = (Byte[])((byte[])reader["movie_image"]);
                        //ms = new MemoryStream(byteBLOBData);
                        //image2.Image = System.Drawing.Image.FromStream(ms);
                        con.Close();
                    }
                }
            }
            return path;
        }
        public string[] getMovieInfo()
        {
            SqlDataReader reader;
            //ArrayList valuesList = new ArrayList();
            string[] info = new string[4];
            //MemoryStream ms=null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["videoRental"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("movieQuery", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@movie_id", this.movieID));
                    cmd.Parameters.Add(new SqlParameter("@StatementType", "GetMovieInfo"));
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        info[0] = reader["movie_title"].ToString();
                        info[1] = reader["movie_genre"].ToString();
                        info[2] = reader["movie_yearReleased"].ToString();
                        info[3] = reader["movie_image"].ToString();
                        con.Close();
                    }
                }
            }
            return info;
        }
    }
}
