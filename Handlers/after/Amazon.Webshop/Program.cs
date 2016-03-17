using System;
using System.IO;
using ColoredConsole;

namespace Amazon.Webshop
{
    class Program
    {
        private static ServiceBus serviceBus;
        private static Guid cartId;

        static void Main(string[] args)
        {
            bool exitMenu = false;
            cartId = Guid.Empty;
            serviceBus = new ServiceBus();

            Console.Clear();
            OutputMenu();

            while (!exitMenu)
            {
                ColorConsole.Write("\n\nMake a choice : ");
                var key = Console.ReadKey();

                switch (key.KeyChar)
                {
                    case '1':
                        cartId = CreateCart();
                        break;
                    case '2':
                        AddMovie(new Guid("70D57FFE-A424-4F80-9632-954EE3027D2B"));
                        break;
                    case '3':
                        AddMovie(new Guid("4470060F-F674-48B2-8FB5-33DFF479CCB0"));
                        break;
                    case '4':
                        FinalizeCart();
                        break;
                    case '5':
                        CancelCart();
                        break;
                    case '6':
                        OutputMenu();
                        break;
                    case 'x':
                        exitMenu = true;
                        break;
                }
            }
        }

        private static void CancelCart()
        {
            if (!IsShoppingCartIdCreated()) return;
            serviceBus.CancelCart(cartId);
        }

        private static void AddMovie(Guid movieId)
        {
            if (!IsShoppingCartIdCreated()) return;
            serviceBus.AddMovieToCart(cartId, movieId);
        }

        private static void FinalizeCart()
        {
            if (!IsShoppingCartIdCreated()) return;
            serviceBus.FinalizeCart(cartId);
        }

        private static Guid CreateCart()
        {
            Guid guid = Guid.NewGuid();
            ColorConsole.WriteLine("\nNew cart created with id : ", guid.ToString().Yellow());
            return guid;
        }

        private static bool IsShoppingCartIdCreated()
        {
            if (cartId == Guid.Empty)
            {
                ColorConsole.WriteLine("\n ! Please create ShoppingCart id first ! ".Red());
                return false;
            }

            return true;
        }

        private static void OutputMenu()
        {
            ColorConsole.WriteLine("===========================================");
            ColorConsole.WriteLine("=                                         =");
            ColorConsole.WriteLine("= ", "          Amazon Webshop               ".Yellow(), " =");
            ColorConsole.WriteLine("=                                         =");
            ColorConsole.WriteLine("===========================================\n\n");
            ColorConsole.WriteLine("Menu :");
            ColorConsole.WriteLine(" 1. ".Red(), "Create a new cart.");
            ColorConsole.WriteLine(" 2. ".Red(), "Add ", "Star Wars : The Force Awakens".Magenta(), " to shoppingcart.");
            ColorConsole.WriteLine(" 3. ".Red(), "Add ", "Deadpool".Magenta(), " to shoppingcart.");
            ColorConsole.WriteLine(" 4. ".Red(), "Pay cart.");
            ColorConsole.WriteLine(" 5. ".Red(), "Cancel cart");
            ColorConsole.WriteLine(" 6. ".Red(), "Rewrite menu");
            ColorConsole.WriteLine(" x. ".Red(), "Exit");
        }
    }
}
