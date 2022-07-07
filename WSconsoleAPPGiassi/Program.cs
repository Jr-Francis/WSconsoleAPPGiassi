using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;



internal class WSconsoleAPPHippo
{
    static void Main(string[] args)
    {
        string linkTxt = @"C:\Users\suele\Desktop\Maria Maria\VStudio22\GiassiLinks.txt";
        string priceTxt = @"C:\Users\suele\Desktop\Maria Maria\VStudio22\GiassiPrecos.txt";
        var listaLinks = new List<string> { };
        var listaPrices = new List<string> { };

        string dadosListaLink;

        if (File.Exists(@"C:\Users\suele\Desktop\Maria Maria\VStudio22\GiassiLinks.txt"))
        {
            using (StreamReader sr = new StreamReader(linkTxt)) //se arquivo n existir voltar p o menu
            {
                Console.WriteLine("Lista de usuários atual:");
                while ((dadosListaLink = sr.ReadLine()) != null)
                {
                    listaLinks.Add(dadosListaLink);
                    Console.WriteLine(dadosListaLink);
                }
                sr.Close();
            }

            IWebDriver driver = new ChromeDriver();

            //PESQUISA PELO IFOOD
            driver.Navigate().GoToUrl("https://www.ifood.com.br/delivery/sao-jose-sc/giassi---campinas-campinas/d46fe054-41ff-42c1-be00-881cbfaac3a0");
            Thread.Sleep(5000);

            foreach (var product in listaLinks)
            {

                driver.Navigate().GoToUrl(product);
                Thread.Sleep(2000);
                //driver.FindElement(By.XPath("/html/body/div[4]/div/div/div/div/button[2]")).Click(); //continuar no endereço


                var selectCepOn = driver.FindElements(By.XPath("/html/body/div[4]/div/div/div/div/button[1]/span"));
                var cepConfirm = driver.FindElements(By.XPath("/html/body/div[4]/div/div/div/div/button[1]"));
                if (selectCepOn.Count > 0)
                {
                    driver.FindElement(By.XPath("/html/body/div[4]/div/div/div/div/button[1]/span")).Click(); //box informar cep
                    Thread.Sleep(2000);
                    driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div/div[1]/div/div/div[2]/div[1]/button[2]")).Click();
                    var cepElement = driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div/div[2]/div/div[1]/div[2]/input"));
                    Thread.Sleep(2000);
                    cepElement.SendKeys("88101300"); //DIGITAR CEP
                    Thread.Sleep(2000);
                    
                    driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div/div[2]/div/div[1]/div[3]/ul/li[1]/div/button")).Click(); //SELECIONAR OPCAO ENDEREÇO
                    //driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div/div[2]/div/div[3]/div/form/label/span[2]/input")).Click(); //SEM NUMERO
                    //driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div/div[2]/div/div[3]/div/form/button")).Click(); //CONFIRMAR BUSCA 
                    driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div/div[3]/div[2]/button")).Click(); //CONFIRMAR LOCALIZAÇÃO
                    driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div/div[3]/div[1]/div[2]/form/div[3]/div/button[1]")).Click(); //FAVORITAR ENDEREÇO
                    driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div/div[3]/div[1]/div[2]/form/div[4]/button")).Click(); //SALVAR ENDEREÇO
                    Thread.Sleep(2000);
                }
                else if (cepConfirm.Count > 0)
                {
                    driver.FindElement(By.XPath("/html/body/div[4]/div/div/div/div/button[1]")).Click();

                }



                Thread.Sleep(6000);
                var productPromoPriceOn = driver.FindElements(By.XPath("/html/body/div[8]/div/div/div/div/section/div[2]/div[3]/div/span/div/span"));
                var productNormalPriceOn = driver.FindElements(By.XPath("/html/body/div[8]/div/div/div/div/section/div[2]/div[3]/div"));
                

                //productNormalPriceOn.Count > 0
                if (productPromoPriceOn.Count > 0) //ok (caso com promoção)
                {
                    var pricePromo = driver.FindElement(By.XPath("/html/body/div[8]/div/div/div/div/section/div[2]/div[3]/div")).Text;
                    var precosArray = pricePromo.Split('-');

                    listaPrices.Add(precosArray[0].Trim());
                    Debug.Print(precosArray[0].Trim());
                    Console.WriteLine(precosArray[0].Trim());

                }
                else if (productPromoPriceOn.Count == 0 && productNormalPriceOn.Count > 0)
                {
                    var priceNormal = driver.FindElement(By.XPath("/html/body/div[8]/div/div/div/div/section/div[2]/div[3]/div"));
                    
                    listaPrices.Add(priceNormal.Text);
                    Debug.Print(priceNormal.Text);
                    Console.WriteLine(priceNormal.Text);
                }

                else
                {
                    Debug.Print("R$ 0,00");
                    listaPrices.Add("R$ 0,00");
                    Console.WriteLine("R$ 0,00");
                }
            }

            using (StreamWriter sw = new StreamWriter(priceTxt))
            {
                for (var aux1 = 0; aux1 < listaPrices.Count; aux1++)
                {
                    sw.WriteLine($"{listaPrices[aux1]}");
                }
                sw.Close();
            }
            Console.WriteLine("OPERAÇÃO CONCLUÍDA!!!");
            Console.ReadKey();
        }

        else
        {
            Console.WriteLine("LISTA INEXISTENTE!");
        }

        Console.ReadKey();

    }

}