using RS_bot.Subcommittees;

namespace RS_bot
{
    internal class MKO : SubComittes
    {
        public MKO(List<string> information) : base(information)
        {
        }

        public override string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string InformationAboutWork { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string TimeWork { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Adress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Longitude { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Latitude { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override List<string> ListWithInfomration => throw new NotImplementedException();

        internal override Task BaseInformationAsync(List<string> information)
        {
            string result = string.Join(", ", information);
            return Task.CompletedTask;
        }

        internal override Task MapPointAsync(double longidue, double latitude)
        {
            throw new NotImplementedException();
        }
    }
}
