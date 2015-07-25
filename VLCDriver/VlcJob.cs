using System.IO;

namespace VLCDriver
{
    public abstract class VlcJob
    {
        public enum JobState { NotStarted = 0, Started = 1, Error = 2 , Finished = 3 }
        public JobState State { get; set; }
        public IVlcInstance Instance { get; set; }
        public FileInfo InputFile { get; set; }
        public FileInfo OutputFile { get; set; }
        public bool QuitAfer { get; set; }
        private const string interfaceStr = "-I dummy";

        public AudioConfiguration AudioConfiguration { get { return audioConfiguration; } protected set { audioConfiguration = value; } }
        private AudioConfiguration audioConfiguration;

        //Todo, Generate New FileName based on input file

        public string GetVlcArguments()
        {
            if (InputFile == null)
                return null;
            const string vlcQuitString = " vlc://quit";

            var transcodeArgs = GetSpecificJobTypeArguments();
            var quitAfter = QuitAfer ? vlcQuitString : string.Empty;
            var arguments = string.Format("{0} \"{1}\" \":sout=#transcode{{{2}}}:std{{dst='{3}',access=file}}\"{4}", interfaceStr ,InputFile.FullName, transcodeArgs, OutputFile.FullName, quitAfter);
            return arguments;
        }

        protected abstract string GetSpecificJobTypeArguments();
    }
}
