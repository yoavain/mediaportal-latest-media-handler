﻿//***********************************************************************
// Assembly         : LatestMediaHandler
// Author           : cul8er
// Created          : 05-09-2010
//
// Last Modified By : cul8er
// Last Modified On : 10-05-2010
// Description      : 
//
// Copyright        : Open Source software licensed under the GNU/GPL agreement.
//***********************************************************************

extern alias RealNLog;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RealNLog.NLog;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace LatestMediaHandler
{
  internal class UtilsFanartHandler
  {
    private static Logger logger = LogManager.GetCurrentClassLogger();

    internal static void SetupFanartHandlerSubcribeScaperFinishedEvent()
    {
      FanartHandler.ExternalAccess.ScraperCompleted +=
        new FanartHandler.ExternalAccess.ScraperCompletedHandler(
          LatestMediaHandlerSetup.TriggerGetLatestMediaInfoOnEvent);
    }

    internal static void DisposeFanartHandlerSubcribeScaperFinishedEvent()
    {
      FanartHandler.ExternalAccess.ScraperCompleted -=
        new FanartHandler.ExternalAccess.ScraperCompletedHandler(
          LatestMediaHandlerSetup.TriggerGetLatestMediaInfoOnEvent);
    }

    internal static string GetFanartForLatest(string tvshow)
    {
      try
      {
        Hashtable ht = FanartHandler.ExternalAccess.GetTVFanart(tvshow);
        if (ht != null && ht.Count > 0)
        {
          return ht[0].ToString();
        }
        return "";
      }
      catch (Exception ex)
      {
        logger.Error("GetFanartForLatest: " + ex.ToString());
        return "";
      }
    }

    internal static string GetMyVideoFanartForLatest(string title)
    {
      try
      {
        return FanartHandler.ExternalAccess.GetMyVideoFanart(title);
      }
      catch (Exception ex)
      {
        logger.Error("GetMyVideoFanartForLatest: " + ex.ToString());
        return null;
      }
    }


    internal static Hashtable GetMusicFanartForLatest(string artist)
    {
      try
      {
        return FanartHandler.ExternalAccess.GetMusicFanartForLatestMedia(artist);
      }
      catch (Exception ex)
      {
        logger.Error("GetMusicFanartForLatest: " + ex.ToString());
        return null;
      }
    }

    internal static string GetFHArtistName(string artist)
    {
      try
      {
        return FanartHandler.ExternalAccess.GetFHArtistName(artist);
      }
      catch (Exception ex)
      {
        logger.Error("GetFHArtistName: " + ex.ToString());
        return null;
      }

    }

    internal static void ScrapeFanartAndThumb(string artist, string album)
    {
      try
      {
        int i = 0;
        while (!FanartHandler.ExternalAccess.ScrapeFanart(artist, album) && i < 12)
        {
          logger.Info("Waiting for new attempt to get fanart and thumb for artist/album (will retry in 5 seconds).");
          Thread.Sleep(5000);
          i++;
        }
      }
      catch (Exception ex)
      {
        logger.Error("ScrapeFanartAndThumb: " + ex.ToString());
      }
    }
  }
}
