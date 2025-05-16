using HobbyManiaManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace HobbyManiaManager.IMDb
{
    public partial class IMDbForm : Form
    {
        private Movie _movie;
        public IMDbForm(Movie movie)
        {
            InitializeComponent();
            _movie = movie;
            string imdbId = movie.imdb_id.ToString();
            string imdbUrl = $"https://www.imdb.com/es-es/title/{imdbId}";
            webView21.Source = new Uri(imdbUrl);
        }
    }
}
