﻿using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using VictorianMoneyCounter.ViewModels;

namespace VictorianMoneyCounter.Views;

public partial class DenominationRow : UserControl, IDenominationRow<DenominationRowViewModel>
{
    private DenominationRowViewModel ViewModel => (DenominationRowViewModel) DataContext;
    
    public DenominationRow()
    {
        InitializeComponent();
    }

    public DenominationRow(DenominationRowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    public DenominationRowViewModel GetViewModel()
    {
        return ViewModel;
    }

}
