using System.Windows.Controls;

namespace RateThisStuff_Client.Controllers
{
    public interface IPageController
    {
        Page Initialize();
        void LoadData();
        void ExecuteNewCommand(object obj);
        void ExecuteEditCommand(object obj);
        void ExecuteSaveCommand(object obj);
        void ExecuteDeleteCommand(object obj);
    }
}
