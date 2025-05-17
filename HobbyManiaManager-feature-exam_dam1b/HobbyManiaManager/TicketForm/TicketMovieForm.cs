using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HobbyManiaManager.Models;
using HobbyManiaManager.Utils;

namespace HobbyManiaManager.TicketForm
{
    public partial class TicketMovieForm : Form
    {
        Customer customer;
        Movie movie;
        Rental rental;
        DateTime Date;
        public TicketMovieForm(Customer customer, Rental rental, Movie movie, DateTime date)
        {
            InitializeComponent();
            this.customer = customer;
            this.movie = movie;
            this.rental = rental;
            this.Date = date;

        }

        private void TicketMovieForm_Load(object sender, EventArgs e)
        {
            CanviarLabels();
        }
        private void CanviarLabels()
        {
            this.lblIdCustomer.Text = $"ID:{customer.Id}";
            this.lblNomCustomer.Text = $"Nom: {customer.Name}";
            this.lblIdMovie.Text = $"Id de la Pelicula: {movie.Id.ToString()}";
            this.lblNomMovie.Text = $"Nom de la Pelicula: {movie.Title}";
            this.lblDataInici.Text = $"Data D'Inici: {rental.StartDate.ToString()}";

            this.lblDataFi.Text = $"Data Fi: {Date.ToString()}";
            var duradasegons = DateTimeUtils.GetDifferenceInSeconds(rental.StartDate, Date);
            var preu = duradasegons * 0.0001;
            this.lblPreu.Text = $"Preu a pagar: {preu}";
        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
