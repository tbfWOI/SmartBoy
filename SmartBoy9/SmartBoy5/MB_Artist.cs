﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBoy
{
    class MB_Artist
    {
        string currentTrackID;
        string content, artist_MBID, artistName, artistGender, artistCountry, artistDOB, artistType, artistBegin = "N/A";

        public void MB_Artist_Lookup(string id)
        {
            currentTrackID = id;
            MB_LookUp();
        }

        private string MB_Content(string url)
        {
            GetWebClient fetcher = new GetWebClient();
            return fetcher.GetWebString(url);
        }


        private void MB_LookUp()
        {
            MB_Lookup_URL_Generator url = new MB_Lookup_URL_Generator();
            content = MB_Content(url.RecordingLookupURL(currentTrackID));

            if (content != "")
            {
                artist_MBID = getBetween(content, "<artist id=\"", "\">");
            }

            if (!check_Artist_MB_ID())
            {
                content = MB_Content(url.ArtistLookupURL(artist_MBID));
                artistLookup();
                MB_Storage();
            }
            if (!check_reln())
                MB_Reln_Storage();
        }


        private void artistLookup()
        {
            artistType = getBetween(content, "<artist type=\"", "\" ");
            artistName = getBetween(content, "<name>", "</name>");
            artistCountry = getBetween(content, "<country>", "</country>");
            if (artistType == "Group")
                artistBegin = getBetween(content, "<begin>", "</begin>");
            else if (artistType == "Person")
            {
                artistGender = getBetween(content, "<gender>", "</gender>");
                artistDOB = getBetween(content, "<begin>", "</begin>");
            }
        }

        private void cleanDate()
        {
            try
            {
                char[] c = artistBegin.ToCharArray();
                StringBuilder s = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    s.Append(c[i]);
                }
                artistBegin = s.ToString();
            }
            catch { }
        }

        private bool checkDate(string date)
        {
            try
            {
                DateTime d = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool check_Artist_MB_ID()
        {
            using (var db = new TestContext())
            {
                db.Database.Connection.Open();
                if (db.Artist_SB.Any(u => u.MB_Artist_ID == artist_MBID))
                    return true;

                return false;
            }

        }

        private bool check_reln()
        {
            using (var db = new TestContext())
            {
                string temp = currentTrackID + artist_MBID;
                if (db.Track_Artist_Reln.Any(u => u.id == temp))
                    return true;

                return false;
            }
        }

        private void MB_Reln_Storage()
        {
            using (var db = new TestContext())
            {
                var reln = new Track_Artist_Reln();
                reln.id = currentTrackID + artist_MBID;
                reln.MB_Track_ID = currentTrackID;
                reln.MB_ArtistID = artist_MBID;
                db.Track_Artist_Reln.Add(reln);
                db.SaveChanges();
            }
        }

        private void MB_Storage()
        {
            using (var db = new TestContext())
            {
                var artist = new Artist_SB();
                artist.MB_Artist_ID = artist_MBID;
                artist.Artist_Name = artistName;
                artist.Artist_Type = artistType;
                if (artistBegin != null)
                {
                    cleanDate();
                    artist.Artist_Begin = artistBegin;
                }
                if (checkDate(artistDOB))
                {
                    artist.Artist_DOB = DateTime.Parse(artistDOB);
                    artist.Artist_Gender = artistGender;
                }
                artist.Artist_Country = artistCountry;

                db.Artist_SB.Add(artist);
                db.SaveChanges();
            }
        }

        private static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            try
            {
                if (strSource.Contains(strStart) && strSource.Contains(strEnd))
                {
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                    End = strSource.IndexOf(strEnd, Start);
                    return strSource.Substring(Start, End - Start);
                }
                else
                {
                    return "N/A";
                }
            }
            catch
            {
                return "N/A";
            }
        }

    }
}