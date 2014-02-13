﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;

namespace SmartBoy
{
    /// <summary>
    /// Interaction logic for LyricsViewer.xaml
    /// </summary>
    public partial class LyricsViewer : Window
    {
        public LyricsViewer()
        {
            InitializeComponent();
            DataContext = new CurrentSongData();
        }
    }
}
