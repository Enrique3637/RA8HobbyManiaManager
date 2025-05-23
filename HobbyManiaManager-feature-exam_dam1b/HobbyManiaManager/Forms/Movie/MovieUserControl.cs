﻿using System;
using System.Drawing;
using System.Globalization;
using System.Reflection.Emit;
using System.Windows.Forms;
using HobbyManiaManager.Forms;
using HobbyManiaManager.IMDb;
using HobbyManiaManager.Models;
using HobbyManiaManager.TicketForm;

namespace HobbyManiaManager
{
    public partial class MovieUserControl : UserControl
    {
        //Aqui estan las clases que necesitamos para realizar los cambios
        private RentalService service;
        private CultureInfo cultureInfo;
        private Movie Movie;
        private RentalService _service;
        private CustomersRepository _customersRepository;
        public Action _refreshAction;

        public MovieUserControl()
        {
            //Muy impostante inicializar service y costumerrespository
            InitializeComponent();
            this.cultureInfo = new CultureInfo("es-ES");
            this._service = new RentalService();
            this._customersRepository = CustomersRepository.Instance;
        }

        public void Load(Movie movie)
        {
            this.Movie = movie;
            this.labelId.Text = $"ID: {Movie.Id.ToString()}";
            this.labelId.AutoSize = true;

            this.labelTitle.Text = $"{Movie.Title}({Movie.ReleaseDate.Year})";
            this.labelTitle.AutoSize = true;

            this.labelGeneros.Text = Movie.GenresAsSting;
            this.labelGeneros.AutoSize = true;

            this.labelOriginalTitle.Text = Movie.OriginalTitle;
            this.labelOriginalTitle.AutoSize = true;

            this.pictureBoxPoster.Load(Movie.PosterUrl(200));

            this.labelOverview.Text = Movie.Overview;
            this.labelOverview.AutoSize = true;
            this.labelVotesCount.Text = $"{Movie.VoteCount.ToString("N0", cultureInfo)} Votes";

            this.pictureBoxAvailable.BorderStyle = BorderStyle.None;

            this.circularProgressBarVotes.Value = (int)Math.Round(Movie.VoteAverage * 10);
            this.circularProgressBarVotes.Text = $"{Math.Round(Movie.VoteAverage * 10)}%";

            this.labelOriginalTitle.Location = new Point(this.labelOriginalTitle.Location.X, this.labelTitle.Bottom + 10);
            this.labelGeneros.Location = new Point(this.labelGeneros.Location.X, this.labelOriginalTitle.Bottom + 5);
            this.circularProgressBarVotes.Location = new Point(this.circularProgressBarVotes.Location.X, this.labelGeneros.Bottom + 10);
            this.labelOverview.Location = new Point(this.circularProgressBarVotes.Right + 10, this.labelGeneros.Bottom + 10);
            this.labelVotesCount.Location = new Point(this.labelVotesCount.Location.X, this.circularProgressBarVotes.Bottom + 5);
            

            CheckAvailability(movie);
            this.Refresh();
        }

        
        private void CheckAvailability(Movie movie)
        {
            bool available = _service.IsAvailable(movie);
            if (available)
            {
                this.pictureBoxAvailable.BackColor = Color.Green;
                this.labelAvailable.Text = "Ready to rent";
                this.buttonStartEndRent.Text = "Start Rent";
            }
            else
            {

                var rental = _service.GetMovieRental(movie.Id);
                var customer = _customersRepository.GetById(rental.CustomerId);
                this.buttonStartEndRent.Text = "End Rent";
                this.pictureBoxAvailable.BackColor = Color.Red;
                this.labelAvailable.Text = $"Not available. Rented by: {customer.Name}({customer.Id})";
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            CheckAvailability(Movie);
        }

        private void buttonStartEndRent_Click(object sender, EventArgs e)
        {
            if (buttonStartEndRent.Text == "End Rent")
            {
                var endDate = DateTime.Now;

                var rental = _service.GetMovieRental(Movie.Id);
                var customer = _customersRepository.GetById(rental.CustomerId);

                _service.FinishRental(customer, Movie, null, endDate);

                var ticketForm = new TicketMovieForm(customer, rental, Movie, endDate);
                ticketForm.ShowDialog();

                _refreshAction?.Invoke();
                this.Refresh(); // Fuerza el redibujo
            }
            else if (buttonStartEndRent.Text == "Start Rent")
            {
                var rentalForm = new RentalForm(Movie, this);
                rentalForm.ShowDialog();

                _refreshAction?.Invoke();
                this.Refresh(); // También refresca después de comenzar un alquiler
            }
        }


        private void MovieUserControl_Load(object sender, EventArgs e)
        {

        }

        private void btnImdb_Click(object sender, EventArgs e)
        {
            string imdb = Movie.imdb_id;
            var imdbForm = new IMDbForm(Movie);
            imdbForm.ShowDialog();
        }
    } 
}
