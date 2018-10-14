namespace Hop.Framework.EFCore.Repository
{
    public static class RepositoryConsts
    {
        /// <summary>
        /// Evitar queries que causem DDoS em banco de dados, deixando trazer 500 registros no máximo.
        /// </summary>
        public const int MaxFindAllRecords = 500;
    }
}