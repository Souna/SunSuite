using NAudio.Wave;
using SunLibrary.SunFileLib.Properties;
using System;
using System.IO;

namespace SunFileManager
{
    public class SunMP3Streamer
    {
        private Stream byteStream;

        private StreamMediaFoundationReader mediaFoundationReader;
        private WaveOutEvent wavePlayer = new WaveOutEvent();
        private WaveFileReader waveFileStream;
        private SunSoundProperty sound;
        private bool repeat;

        private bool playbackSuccessfully = true;

        public SunMP3Streamer(SunSoundProperty sound, bool repeat)
        {
            this.repeat = repeat;
            this.sound = sound;
            byteStream = new MemoryStream(sound.GetBytes(false));

            try
            {
                mediaFoundationReader = new StreamMediaFoundationReader(byteStream);
                wavePlayer.Init(mediaFoundationReader);
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
                if (mediaFoundationReader != null)
                    mediaFoundationReader.Seek(0, SeekOrigin.Begin);
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
            if (mediaFoundationReader != null)
            {
                mediaFoundationReader.Dispose();
                mediaFoundationReader = null;
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
                if (mediaFoundationReader != null)
                    return (int)(mediaFoundationReader.Position / mediaFoundationReader.WaveFormat.AverageBytesPerSecond);
                else if (waveFileStream != null)
                    return (int)(waveFileStream.Position / waveFileStream.WaveFormat.AverageBytesPerSecond);

                return 0;
            }
            set
            {
                if (mediaFoundationReader != null)
                    mediaFoundationReader.Seek(value * mediaFoundationReader.WaveFormat.AverageBytesPerSecond, SeekOrigin.Begin);
                else if (waveFileStream != null)
                    waveFileStream.Seek(value * waveFileStream.WaveFormat.AverageBytesPerSecond, SeekOrigin.Begin);
            }
        }
    }
}