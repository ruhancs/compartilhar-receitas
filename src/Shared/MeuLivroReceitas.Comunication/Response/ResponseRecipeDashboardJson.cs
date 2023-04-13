namespace MeuLivroReceitas.Comunication.Response
{
    //utilizado em ResponseDashboardJson
    //para criar uma List de ResponseRecipeDashboardJson
    public class ResponseRecipeDashboardJson
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string IngredientsQuantity { get; set; }
        public int PreparationTime { get; set; }
    }
}
