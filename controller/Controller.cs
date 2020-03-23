using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using musicDBManagementSystem.project.repository;

namespace musicDBManagementSystem.project.controller {
    public class Controller {
        private DataBaseRepository repository;
        public Controller(DataBaseRepository repo) {
            this.repository = repo;
        }

        public void close() {
            this.repository.close();
        }

        public DataRowCollection getFormationRows() {
            return this.repository.selectFromTableWhere("title, about", "Formation", null);
        }
 
        public int getFormationId(string formationName) {
            return int.Parse(this.repository.selectFromTableWhere("id",
                "Formation", $"title = '{formationName}'")[0][0].ToString());
        }
        
        public DataRowCollection getGenreRows() {
            return this.repository.selectFromTableWhere("title, about", "Genre", null);
        }

        public int getGenreId(string genreName) {
            return int.Parse(this.repository.selectFromTableWhere("id",
                "Genre", $"title = '{genreName}'")[0][0].ToString());
        }

        public DataRowCollection getArtistRows() {
            return this.repository.selectFromTableWhere("title", "Artist", null);
        }
        
        public int getArtistId(string artistName) {
            return int.Parse(this.repository.selectFromTableWhere("id",
                "Artist", $"title = '{artistName}'")[0][0].ToString());
        }

        public DataRowCollection getArtistsOfGenre(string genreName) {
            return this.repository.selectFromTableWhere("Artist.title",
                "Artist inner join Genre on Artist.genreId = Genre.id",
                $"Genre.title = '{genreName}'");
        }

        public DataRowCollection getArtistsOfFormation(string formationName) {
            return this.repository.selectFromTableWhere("Artist.title",
                "Artist inner join Formation on Artist.formationId = Formation.id",
                $"Formation.title = '{formationName}'");
        }

        public DataRowCollection getArtistsOfFormationAndGenre(List<int> formationIds, List<int> genreIds) {
            string where = null;
            if (formationIds.Count > 0) {
                where += "(";
                for (int index = 0; index < formationIds.Count - 1; index += 1) {
                    where += $"Formation.id='{formationIds[index].ToString()}' or ";
                }  where += $"Formation.id='{formationIds[formationIds.Count - 1]}'";
                where += ")";
                if (genreIds.Count > 0) where += " and ";
            }

            if (genreIds.Count > 0) {
                where += $"(Genre.id='{genreIds[0].ToString()}'";
                for (int index = 1; index < genreIds.Count; index += 1) {
                    where += $" or Genre.id='{genreIds[index].ToString()}'";
                }
                where += ")";
            }

            return this.repository.selectFromTableWhere("Artist.title",
                "Artist inner join Genre on Artist.genreId = Genre.id " +
                "inner join Formation on Artist.formationId = Formation.id",
                where);
        }

        public void addArtist(string title, int formationId, int genreId) {
            this.repository.insertIntoTableValues("Artist", null,
                new List<string> {$"('{title}', '{formationId}', '{genreId}')"});
        }

        public void updateArtist(int artistId, string title, int formationId, int genreId) {
            this.repository.updateTableSetWhere("Artist", $"title='{title}', formationId='{formationId}', genreId='{genreId}'", $"id='{artistId}'");
        }

        public void removeArtist(int artistId) {
            this.repository.deleteFromTableWhere("Artist", $"id='{artistId}'");
        }
    }
}