using NAudio.Wave;
using SunLibrary.SunFileLib.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunFileManager
{
    public class SunMP3Streamer
    {
        private Stream byteStream;

        private Mp3FileReader mpegStream;
        private WaveFileReader waveFileStream;

        private WaveOut wavePlayer;
        private SunSoundProperty sound;
        private bool repeat;

        private bool playbackSuccessfully = true;

        public SunMP3Streamer(SunSoundProperty sound, bool repeat)
        {
            this.repeat = repeat;
            this.sound = sound;
            byteStream = new MemoryStream(sound.GetBytes(false));

            wavePlayer = new WaveOut(WaveCallbackInfo.FunctionCallback());
            try
            {
                mpegStream = new Mp3FileReader(byteStream);
                wavePlayer.Init(mpegStream);
            }
            catch (System.InvalidOperationException)
            {
                try
                {
                    waveFileStream = new WaveFileReader(byteStream);
                    wavePlayer.Init(waveFileStream);
                }
                catch (FormatException)
                {
                    playbackSuccessfully = false;
                }
                //InvalidDataException
            }
            Volume = 0.5f; // default volume
            wavePlayer.PlaybackStopped += new EventHandler<StoppedEventArgs>(wavePlayer_PlaybackStopped);
        }

        private void wavePlayer_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (repeat && !disposed)
            {
                if (mpegStream != null)
                    mpegStream.Seek(0, SeekOrigin.Begin);
                else
                    waveFileStream.Seek(0, SeekOrigin.Begin);

                wavePlayer.Pause();
                wavePlayer.Play();
            }
        }

        private bool disposed = false;

        public bool Disposed
        {
            get { return disposed; }
        }

        public void Dispose()
        {
            if (!playbackSuccessfully)
                return;

            disposed = true;
            wavePlayer.Dispose();
            if (mpegStream != null)
            {
                mpegStream.Dispose();
                mpegStream = null;
            }
            if (waveFileStream != null)
            {
                waveFileStream.Dispose();
                waveFileStream = null;
            }
            byteStream.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void Play()
        {
            if (!playbackSuccessfully)
                return;

            wavePlayer.Play();
        }

        public void Pause()
        {
            if (!playbackSuccessfully)
                return;

            wavePlayer.Pause();
        }

        public void Stop()
        {
            if (!playbackSuccessfully) return;
            wavePlayer.Stop();
        }

        public bool Repeat
        {
            get { return repeat; }
            set { repeat = value; }
        }

        public int Length
        {
            get { return sound.Length / 1000; }
        }

        public float Volume
        {
            get
            {
                return wavePlayer.Volume;
            }
            set
            {
                if (value >= 0 && value <= 1.0)
                {
                    this.wavePlayer.Volume = value;
                }
            }
        }

        public int Position
        {
            get
            {
                if (mpegStream != null)
                    return (int)(mpegStream.Position / mpegStream.WaveFormat.AverageBytesPerSecond);
                else if (waveFileStream != null)
                    return (int)(waveFileStream.Position / waveFileStream.WaveFormat.AverageBytesPerSecond);

                return 0;
            }
            set
            {
                if (mpegStream != null)
                    mpegStream.Seek(value * mpegStream.WaveFormat.AverageBytesPerSecond, SeekOrigin.Begin);
                else if (waveFileStream != null)
                    waveFileStream.Seek(value * waveFileStream.WaveFormat.AverageBytesPerSecond, SeekOrigin.Begin);
            }
        }
    }
}