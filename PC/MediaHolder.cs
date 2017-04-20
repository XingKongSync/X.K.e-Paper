using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC
{
    public class MediaHolder : IComparable<MediaHolder>
    {
        static Random s_Rand = new Random();

        WMPLib.IWMPMedia mMedia;

        public MediaHolder(WMPLib.IWMPMedia media)
        {
            mMedia = media;
        }

        public WMPLib.IWMPMedia Media
        {
            get { return mMedia; }
        }

        string m_album;
        public string Album
        {
            get
            {
                if (m_album == null)
                {
                    m_album = mMedia.getItemInfo("WM/AlbumTitle");
                    if (m_album == null) m_album = string.Empty;
                }
                return m_album;
            }
        }

        int m_track;
        public int Track
        {
            get
            {
                if (m_track == 0)
                {
                    string track = mMedia.getItemInfo("WM/TrackNumber");
                    if (track == null) track = string.Empty;
                    if (!int.TryParse(track, out m_track)) m_track = -1;
                }
                return m_track;
            }
        }

        String m_title;
        public String Title
        {
            get
            {
                if (m_title == null)
                {
                    m_title = mMedia.getItemInfo("Title");
                    if (m_title == null) m_title = string.Empty;
                }
                return m_title;
            }
        }

        string m_Artist;
        public string Artist
        {
            get
            {
                if (m_Artist == null)
                {
                    m_Artist = mMedia.getItemInfo("Artist");
                    if (m_Artist == null)
                    {
                        m_Artist = string.Empty;
                    }
                }
                return m_Artist;
            }
        }

        int m_randomOrdinal = 0;
        public int RandomOrdinal
        {
            get
            {
                if (m_randomOrdinal == 0)
                {
                    m_randomOrdinal = s_Rand.Next();
                }
                return m_randomOrdinal;
            }
        }

        public int SpecialOrdinal { get; set; }

        public bool IsSameAs(MediaHolder other)
        {
            // Windows does some weird rounding. The same track can show up with nearly a second difference in duration
            // So we look for values between -1 and 1
            double diff = mMedia.duration - other.mMedia.duration;

            return diff > -1 && diff < 1
                && Track == other.Track
                && string.Equals(Title, other.Title, StringComparison.Ordinal)
                && string.Equals(Album, other.Album, StringComparison.Ordinal);
        }

        public static int CompareAlbumRandom(MediaHolder a, MediaHolder b)
        {
            int comp = String.Compare(a.Album, b.Album, StringComparison.Ordinal);
            if (comp != 0) return comp;
            return a.RandomOrdinal - b.RandomOrdinal;
        }

        /// <summary>
        /// Default comparer uses the traditional sort order: Album, Track, Title
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(MediaHolder other)
        {
            int comp = String.Compare(Album, other.Album, StringComparison.Ordinal);
            if (comp != 0) return comp;
            comp = Track - other.Track;
            if (comp != 0) return comp;
            return String.Compare(Title, other.Title);
        }
    }
}
