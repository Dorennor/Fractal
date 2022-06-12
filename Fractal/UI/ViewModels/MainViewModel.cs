using Fractal.UI.Core;
using Fractal.UI.ViewModels.LR1;
using Fractal.UI.ViewModels.LR2;
using Fractal.UI.ViewModels.LR3;
using IndividualFirstViewModel = Fractal.UI.ViewModels.LR1.IndividualFirstViewModel;
using IndividualSecondViewModel = Fractal.UI.ViewModels.LR1.IndividualSecondViewModel;

namespace Fractal.UI.ViewModels;

public class MainViewModel : ObservableObject
{
    #region LR1

    public RelayCommand TriangleViewModelCommand { get; set; }
    public RelayCommand SquareViewModelCommand { get; set; }
    public RelayCommand LR1IndividualFirstViewModelCommand { get; set; }
    public RelayCommand LR1IndividualSecondViewModelCommand { get; set; }

    public TriangleViewModel TriangleViewModel { get; set; }
    public SquareViewModel SquareViewModel { get; set; }
    public LR1.IndividualFirstViewModel LR1IndividualFirstViewModel { get; set; }
    public LR1.IndividualSecondViewModel LR1IndividualSecondViewModel { get; set; }

    #endregion LR1

    #region LR2

    public RelayCommand SnowflakeViewModelCommand { get; set; }
    public RelayCommand LR2IndividualFirstViewModelCommand { get; set; }
    public RelayCommand LR2IndividualSecondViewModelCommand { get; set; }

    public SnowflakeViewModel SnowflakeViewModel { get; set; }
    public LR2.IndividualFirstViewModel LR2IndividualFirstViewModel { get; set; }
    public LR2.IndividualSecondViewModel LR2IndividualSecondViewModel { get; set; }

    #endregion LR2

    #region LR3

    public RelayCommand MandelbortViewModelCommand { get; set; }
    public RelayCommand LR3IndividualFirstViewModelCommand { get; set; }
    public RelayCommand LR3IndividualSecondViewModelCommand { get; set; }

    public MandelbortViewModel MandelbortViewModel { get; set; }
    public LR3.IndividualFirstViewModel LR3IndividualFirstViewModel { get; set; }
    public LR3.IndividualSecondViewModel LR3IndividualSecondViewModel { get; set; }

    #endregion LR3

    private object _currentView;

    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        #region LR1

        TriangleViewModel = new TriangleViewModel();
        SquareViewModel = new SquareViewModel();
        LR1IndividualFirstViewModel = new IndividualFirstViewModel();
        LR1IndividualSecondViewModel = new IndividualSecondViewModel();

        TriangleViewModelCommand = new RelayCommand(o =>
        {
            CurrentView = TriangleViewModel;
        });

        SquareViewModelCommand = new RelayCommand(o =>
        {
            CurrentView = SquareViewModel;
        });

        LR1IndividualFirstViewModelCommand = new RelayCommand(o =>
        {
            CurrentView = LR1IndividualFirstViewModel;
        });

        LR1IndividualSecondViewModelCommand = new RelayCommand(o =>
        {
            CurrentView = LR1IndividualSecondViewModel;
        });

        #endregion LR1

        #region LR2

        SnowflakeViewModel = new SnowflakeViewModel();
        LR2IndividualFirstViewModel = new LR2.IndividualFirstViewModel();
        LR2IndividualSecondViewModel = new LR2.IndividualSecondViewModel();

        SnowflakeViewModelCommand = new RelayCommand(o =>
        {
            CurrentView = SnowflakeViewModel;
        });

        LR2IndividualFirstViewModelCommand = new RelayCommand(o =>
        {
            CurrentView = LR2IndividualFirstViewModel;
        });

        LR2IndividualSecondViewModelCommand = new RelayCommand(o =>
        {
            CurrentView = LR2IndividualSecondViewModel;
        });

        #endregion LR2

        #region LR3

        MandelbortViewModel = new MandelbortViewModel();
        LR3IndividualFirstViewModel = new LR3.IndividualFirstViewModel();
        LR3IndividualSecondViewModel = new LR3.IndividualSecondViewModel();

        MandelbortViewModelCommand = new RelayCommand(o =>
        {
            CurrentView = MandelbortViewModel;
        });

        LR3IndividualFirstViewModelCommand = new RelayCommand(o =>
        {
            CurrentView = LR3IndividualFirstViewModel;
        });

        LR3IndividualSecondViewModelCommand = new RelayCommand(o =>
        {
            CurrentView = LR3IndividualSecondViewModel;
        });

        #endregion LR3
    }
}