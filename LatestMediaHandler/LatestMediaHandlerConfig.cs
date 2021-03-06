﻿//***********************************************************************
// Assembly         : LatestMediaHandler
// Author           : cul8er
// Created          : 05-09-2010
//
// Last Modified By : ajs
// Last Modified On : 30-09-2015
// Description      : 
//
// Copyright        : Open Source software licensed under the GNU/GPL agreement.
//***********************************************************************
extern alias LMHNLog;

using System;
using System.IO;
using System.Windows.Forms;

using LMHNLog.NLog;
using LMHNLog.NLog.Config;
using LMHNLog.NLog.Targets;

using MediaPortal.Configuration;
using MediaPortal.Profile;

namespace LatestMediaHandler
{
  partial class LatestMediaHandlerConfig : Form
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();
    private const string LogFileName = "LatestMediaHandler_config.log";
    private const string OldLogFileName = "LatestMediaHandler_config.bak";

    public LatestMediaHandlerConfig()
    {
      InitializeComponent();
    }

    private void SetupConfigFile()
    {
      /*
      try
      {
        String path = Config.GetFile(Config.Dir.Config, "LatestMediaHandler.xml");
        String pathOrg = Config.GetFile(Config.Dir.Config, "LatestMediaHandler.org");
        if (File.Exists(path))
        {
          //do nothing
        }
        else
        {
          File.Copy(pathOrg, path);
        }
      }
      catch (Exception ex)
      {
        logger.Error("setupConfigFile: " + ex);
      }
      */
    }

    private string GetTVSeriesRatings()
    {
      string s = string.Empty;
      for (int i = 0; i < checkedListBox1.Items.Count; i++)
      {
        string isChecked = string.Empty;
        if (checkedListBox1.GetItemChecked(i))
        {
          isChecked = "1";
        }
        else
        {
          isChecked = "0";
        }

        if (i == 0)
        {
          s = isChecked;
        }
        else
        {
          s = s + ";" + isChecked;
        }
      }
      return s;
    }

    private void DoSave()
    {
      try
      {
        Utils.LatestPictures = checkBox5.Checked;
        Utils.LatestMusic = checkBox6.Checked;
        Utils.LatestMyVideos = checkBox9.Checked;
        Utils.LatestMyVideosWatched = checkBox8.Checked;
        Utils.LatestMovingPictures = checkBox7.Checked;
        Utils.LatestMovingPicturesWatched = checkBox10.Checked;
        Utils.LatestTVSeries = checkBox2.Checked;
        Utils.LatestTVSeriesWatched = checkBox11.Checked;
        Utils.LatestTVSeriesRatings = GetTVSeriesRatings();
        Utils.LatestTVSeriesType = comboBoxTVSeriesType.SelectedIndex;
        Utils.LatestTVSeriesView = comboBoxTVSeriesMode.SelectedIndex;
        Utils.LatestTVRecordings = checkBox3.Checked;
        Utils.LatestTVRecordingsWatched = checkBox14.Checked;
        Utils.LatestTVRecordingsUnfinished = checkBoxRecordingsUnfinished.Checked;
        Utils.LatestMyFilms = checkBox1.Checked;
        Utils.LatestMyFilmsWatched = checkBox4.Checked;
        Utils.RefreshDbPicture = checkBox12.Checked;
        Utils.RefreshDbMusic = checkBox13.Checked;
        Utils.LatestMvCentral = checkBox15.Checked;
        Utils.LatestMvCentralThumbType = comboBoxMvCThumbType.SelectedIndex + 1;
      }
      catch (Exception ex)
      {
        logger.Error("DoSave: " + ex);
      }
      try
      {
        Utils.ReorgInterval = comboBox1.SelectedItem.ToString();
        Utils.LatestMusicType = Utils.StringToMusicType(comboBox2.SelectedItem.ToString());
        Utils.DateFormat = comboBox3.SelectedItem.ToString();
      }
      catch (Exception ex)
      {
        logger.Error("DoSave: " + ex);
      }

      Utils.SaveSettings();

      // MessageBox.Show("Settings is stored in memory. Make sure to press Ok when exiting MP Configuration. Pressing Cancel when exiting MP Configuration will result in these setting NOT being saved!");
      MessageBox.Show(Translation.PrefsSaveChangesMsgBox);
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      DoSave();
    }

    private void LatestMediaHandlerConfig_FormClosing(object sender, FormClosedEventArgs e)
    {
      if (!DesignMode)
      {
        // DialogResult result = MessageBox.Show("Do you want to save your changes?", 
        //                                       "Save Changes?",
        //                                       MessageBoxButtons.YesNo);
        DialogResult result = MessageBox.Show(Translation.PrefsSaveChangesDialog, 
                                              Translation.PrefsSaveChanges,
                                              MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)      
        {
          DoSave();
        }
        logger.Info("Latest Media Handler configuration is stopped.");
        this.Close();
      }
    }

    private void LatestMediaHandlerConfig_WindowInit()
    {
      comboBox1.Items.Add("30");
      comboBox1.Items.Add("60");
      comboBox1.Items.Add("120");
      comboBox1.Items.Add("240");
      comboBox1.Items.Add("480");
      comboBox1.Items.Add("720");
      comboBox1.Items.Add("1440");

      comboBox3.Items.Add("yyyy-MM-dd");
      comboBox3.Items.Add("dd.MM.yyyy");
      comboBox3.Items.Add("MM/dd/yyyy");
      comboBox3.Items.Add("dd/MM/yyyy");
      comboBox3.Items.Add("MM/dd/yy");
      comboBox3.Items.Add("dd/MM/yy");

      /*
      comboBox2.Items.Add("Latest Added Music");
      comboBox2.Items.Add("Latest Played Music");
      comboBox2.Items.Add("Most Played Music");
      */
      comboBox2.Items.Add(Translation.PrefsLatestAddedMusic);
      comboBox2.Items.Add(Translation.PrefsLatestPlayedMusic);
      comboBox2.Items.Add(Translation.PrefsMostPlayedMusic);

      comboBoxTVSeriesType.Items.Add(Translation.PrefsLatestEpisodes);
      comboBoxTVSeriesType.Items.Add(Translation.PrefsLatestSeasons);
      comboBoxTVSeriesType.Items.Add(Translation.PrefsLatestSeries);

      comboBoxTVSeriesMode.Items.Add(Translation.PrefsViewLatestAdded);
      comboBoxTVSeriesMode.Items.Add(Translation.PrefsViewLatestWatched);
      comboBoxTVSeriesMode.Items.Add(Translation.PrefsViewHighestRated);
      comboBoxTVSeriesMode.Items.Add(Translation.PrefsViewNextEpisodes);

      comboBoxMvCThumbType.Items.Add(Translation.PrefsMvCThumbArtist);
      comboBoxMvCThumbType.Items.Add(Translation.PrefsMvCThumbAlbum);
      comboBoxMvCThumbType.Items.Add(Translation.PrefsMvCThumbTrack);
      /*
      checkedListBox1.Items.Add("TV-Y: This program is designed to be appropriate for all children");
      checkedListBox1.Items.Add("TV-Y7: This program is designed for children age 7 and above.");
      checkedListBox1.Items.Add("TV-G: Most parents would find this program suitable for all ages.");
      checkedListBox1.Items.Add("TV-PG: This program contains material that parents may find unsuitable for younger children.");
      checkedListBox1.Items.Add("TV-14: This program contains some material that many parents would find unsuitable for children under 14 years of age.");
      checkedListBox1.Items.Add("TV-MA: This program is specifically designed to be viewed by adults and therefore may be unsuitable for children under 17.");
      */
      label3.Text =  Translation.PrefsRatingDesc;

      checkedListBox1.Items.Add(Translation.PrefsRatingTV_Y);
      checkedListBox1.Items.Add(Translation.PrefsRatingTV_Y7);
      checkedListBox1.Items.Add(Translation.PrefsRatingTV_G);
      checkedListBox1.Items.Add(Translation.PrefsRatingTV_PG);
      checkedListBox1.Items.Add(Translation.PrefsRatingTV_14);
      checkedListBox1.Items.Add(Translation.PrefsRatingTV_MA);
      // 
      tabLMH.Text = Translation.PrefsTabLMH;
      tabAbout.Text = Translation.PrefsTabAbout;
      richTextBox1.AppendText(Translation.PrefsDescription.Replace("\r\n", Environment.NewLine).Replace("\n", Environment.NewLine));
      //
      groupBox11.Text = Translation.PrefsLMHOptions ;
      groupBox13.Text = Translation.PrefsUpdateDB;
      groupBox1.Text = Translation.PrefsMiscOptions;
      //
      label31.Text = Translation.PrefsLMHOptionsDesc;
      label4.Text = Translation.PrefsDateFormat;
      label2.Text =  Translation.PrefsMinutes;
      checkBox7.Text =  Translation.PrefsMovingPictures;
      checkBox4.Text =  Translation.PrefsMovingPicturesWatched;
      checkBox6.Text =  Translation.PrefsMusic;
      checkBox13.Text =  Translation.PrefsMusic;
      checkBox15.Text =  Translation.PrefsMvCentral;
      checkBox1.Text =  Translation.PrefsMyFilms;
      checkBox4.Text =  Translation.PrefsMyFilmsWatched;
      checkBox9.Text =  Translation.PrefsMyVideos;
      checkBox8.Text =  Translation.PrefsMyVideosWatched;
      checkBox12.Text =  Translation.PrefsPictures;
      checkBox5.Text =  Translation.PrefsPictures;
      checkBox3.Text =  Translation.PrefsRecordings;
      checkBox14.Text =  Translation.PrefsRecordingsWatched;
      checkBoxRecordingsUnfinished.Text = Translation.PrefsRecordingsUnfinished;
      label1.Text =  Translation.PrefsRefreshInterval;
      checkBox2.Text =  Translation.PrefsTVSeries;
      checkBox11.Text =  Translation.PrefsTVSeriesWatched;
      label35.Text =  Translation.PrefsUpdateDBDesc;
    }

    private void LatestMediaHandlerConfig_Load(object sender, EventArgs e)
    {
      try
      {
        InitLogger();
        logger.Info("Latest Media Handler configuration is starting.");
        logger.Info("Latest Media Handler version is " + Utils.GetAllVersionNumber());
      }
      catch (Exception ex)
      {
        logger.Error("LatestMediaHandlerConfig_Load: " + ex);
      }

      label11.Text = "Version " + Utils.GetAllVersionNumber();
      Text = Text + " v" + Utils.GetAllVersionNumber();

      Translation.Init();
      LatestMediaHandlerConfig_WindowInit();

      SetupConfigFile();
      Utils.LoadSettings(true);

      if (!string.IsNullOrEmpty(Utils.LatestTVSeriesRatings))
      {
        string[] s = Utils.LatestTVSeriesRatings.Split(';');
        for (int i = 0; i < s.Length; i++)
        {
          checkedListBox1.SetItemChecked(i, s[i].Equals("1"));
        }
      }
      else
      {
        for (int i = 0; i < checkedListBox1.Items.Count; i++)
        {
          checkedListBox1.SetItemChecked(i, true);
        }
      }

      if (!string.IsNullOrEmpty(Utils.DateFormat))
      {
        comboBox3.SelectedItem = Utils.DateFormat;
      }
      else
      {
        comboBox3.SelectedItem = "yyyy-MM-dd";
      }

      if (!string.IsNullOrEmpty(Utils.ReorgInterval))
      {
        comboBox1.SelectedItem = Utils.ReorgInterval;
      }
      else
      {
        comboBox1.SelectedItem = "1440";
      }

      comboBox2.SelectedItem = Utils.MusicTypeToConfig(Utils.LatestMusicType);

      checkBox1.Checked = Utils.LatestMyFilms;
      checkBox5.Checked = Utils.LatestPictures;
      checkBox6.Checked = Utils.LatestMusic;
      checkBox9.Checked = Utils.LatestMyVideos;
      checkBox15.Checked = Utils.LatestMvCentral;

      if (Utils.LatestMvCentralThumbType > 0 && Utils.LatestMvCentralThumbType <= 3)
      {
        comboBoxMvCThumbType.SelectedIndex = Utils.LatestMvCentralThumbType - 1;
      }
      else
      {
        comboBoxMvCThumbType.SelectedIndex = 0;
      }

      checkBox8.Checked = Utils.LatestMyVideosWatched;
      checkBox7.Checked = Utils.LatestMovingPictures;
      checkBox10.Checked = Utils.LatestMovingPicturesWatched;
      checkBox2.Checked = Utils.LatestTVSeries;
      checkBox11.Checked = Utils.LatestTVSeriesWatched;
      comboBoxTVSeriesMode.SelectedIndex = Utils.LatestTVSeriesView;
      comboBoxTVSeriesType.SelectedIndex = Utils.LatestTVSeriesType;
      checkBox12.Checked = Utils.RefreshDbPicture;
      checkBox13.Checked = Utils.RefreshDbMusic;
      checkBox3.Checked = Utils.LatestTVRecordings;
      checkBox14.Checked = Utils.LatestTVRecordingsWatched;
      checkBoxRecordingsUnfinished.Checked = Utils.LatestTVRecordingsUnfinished;
      checkBox4.Checked = Utils.LatestMyFilmsWatched;

      try
      {
        logger.Info("Latest Media Handler configuration is started.");
      }
      catch (Exception ex)
      {
        logger.Error("LatestMediaHandlerConfig_Load: " + ex);
      }

    }

    /// <summary>
    /// Setup logger. This funtion made by the team behind Moving Pictures 
    /// (http://code.google.com/p/moving-pictures/)
    /// </summary>
    private void InitLogger()
    {
      //LoggingConfiguration config = new LoggingConfiguration();
      LoggingConfiguration config = LogManager.Configuration ?? new LoggingConfiguration();

      try
      {
        FileInfo logFile = new FileInfo(Config.GetFile(Config.Dir.Log, LogFileName));
        if (logFile.Exists)
        {
          if (File.Exists(Config.GetFile(Config.Dir.Log, OldLogFileName)))
            File.Delete(Config.GetFile(Config.Dir.Log, OldLogFileName));

          logFile.CopyTo(Config.GetFile(Config.Dir.Log, OldLogFileName));
          logFile.Delete();
        }
      }
      catch (Exception)
      {
      }

      FileTarget fileTarget = new FileTarget();
      fileTarget.FileName = Config.GetFile(Config.Dir.Log, LogFileName);
      fileTarget.Encoding = "utf-8";
      fileTarget.Layout = "${date:format=dd-MMM-yyyy HH\\:mm\\:ss} " +
                          "${level:fixedLength=true:padding=5} " +
                          "[${logger:fixedLength=true:padding=20:shortName=true}]: ${message} " +
                          "${exception:format=tostring}";

      config.AddTarget("latestmedia-handler", fileTarget);

      // Get current Log Level from MediaPortal 
      LogLevel logLevel = LogLevel.Debug;
      int intLogLevel = 3;

      using (Settings xmlreader = new MPSettings())
      {
        intLogLevel = xmlreader.GetValueAsInt("general", "loglevel", intLogLevel);
      }

      switch (intLogLevel)
      {
        case 0:
          logLevel = LogLevel.Error;
          break;
        case 1:
          logLevel = LogLevel.Warn;
          break;
        case 2:
          logLevel = LogLevel.Info;
          break;
        default:
          logLevel = LogLevel.Debug;
          break;
      }

      #if DEBUG
      logLevel = LogLevel.Debug;
      #endif

      LoggingRule rule = new LoggingRule("*", logLevel, fileTarget);
      config.LoggingRules.Add(rule);

      LogManager.Configuration = config;
    }

    private void checkBox3_CheckedChanged(object sender, EventArgs e)
    {
      if (checkBox3.Checked)
      {
        checkBox14.Enabled = true;
      }
      else
      {
        checkBox14.Enabled = false;
      }
    }

    private void checkBox7_CheckedChanged(object sender, EventArgs e)
    {
      if (checkBox7.Checked)
      {
        checkBox10.Enabled = true;
      }
      else
      {
        checkBox10.Enabled = false;
      }
    }

    private void checkBox2_CheckedChanged(object sender, EventArgs e)
    {
      if (checkBox2.Checked)
      {
        checkBox11.Enabled = true;
      }
      else
      {
        checkBox11.Enabled = false;
      }
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      Utils.ReorgInterval = comboBox1.SelectedItem.ToString();
    }

    private void checkBox13_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void groupBox1_Enter(object sender, EventArgs e)
    {

    }


    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      if (checkBox1.Checked)
      {
        checkBox4.Enabled = true;
      }
      else
      {
        checkBox4.Enabled = false;
      }
    }

    private void checkBox5_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void checkBox6_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
      Utils.LatestMusicType = Utils.StringToMusicType(comboBox2.SelectedItem.ToString());
    }

    private void label3_Click(object sender, EventArgs e)
    {

    }

    private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
    {
      Utils.DateFormat = comboBox3.SelectedItem.ToString();
    }

    private void checkBox9_CheckedChanged(object sender, EventArgs e)
    {

    }
  }
}
