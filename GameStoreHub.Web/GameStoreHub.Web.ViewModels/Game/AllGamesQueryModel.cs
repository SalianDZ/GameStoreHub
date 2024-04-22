using GameStoreHub.Web.ViewModels.Game.Enums;
using System.ComponentModel.DataAnnotations;
using static GameStoreHub.Common.EntityValidationConstants.GeneralApplicationConstants;

namespace GameStoreHub.Web.ViewModels.Game
{
	public class AllGamesQueryModel
	{
        public AllGamesQueryModel()
        {
            Categories = new HashSet<string>();
            Games = new HashSet<GamesViewModel>();
            CurrentPage = DefaultPage;
            GamesPerPage = EntitiesPerPage;
        }

        public string? Category { get; set; }

        [Display(Name = "Sort Games By")]
        public GameSorting GameSorting { get; set; }

        public int CurrentPage { get; set; }

        public int GamesPerPage { get; set; }

        public int TotalGames { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<GamesViewModel> Games { get; set; }
    }
}
