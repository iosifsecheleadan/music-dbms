using System.Windows.Forms;
using musicDBManagementSystem.project.controller;
using musicDBManagementSystem.project.repository;


/*
The application must contain a form allowing the user to manipulate data in 2 tables that are in a 1:n relationship (parent table and child table). 
The  application must provide the following functionalities:
    -display all the records in the parent table;
    -display the child records for a specific (i.e., selected) parent record;
    -add / remove / update child records for a specific parent record.
You must  use the DataSet and SqlDataAdapter classes.
You  are free to use any controls on the form.
 */

namespace musicDBManagementSystem.project {
    public static class Program{
        public static void Main(string[] args) {
            DataBaseRepository repo = new DataBaseRepository("172.17.0.2", "music", "SA", "Hheren1999");
            Controller ctrl = new Controller(repo);
            Graphic graphic = new Graphic(ctrl, "Music DBMS");
            Application.Run(graphic.getMain());
            repo.close();
        }
    }
}

