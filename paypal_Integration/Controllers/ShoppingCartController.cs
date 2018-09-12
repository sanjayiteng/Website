using PayPalIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PayPalIntegration.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart

        public int IsExisting(int Id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].pr.ProductId == Id)
                    return i;
            }
            return -1;
        }

        public ActionResult Delete(int Id)
        {

            int index = IsExisting(Id);
            List<Item> cart = (List<Item>)Session["cart"];
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return View("Cart");
        }
        public ActionResult OrderNow(int Id)
        {
            if (Session["cart"] == null)
            {

                List<Item> cart = new List<Item>();
                Session["cart"] = cart;
                List<Product> projuctLst = (List<Product>)(Session["Project"]);
                Product product = projuctLst.Where(x => x.ProductId == Id).FirstOrDefault();
                cart.Add(new Item() { pr = product, Quantity = 1 });

            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = IsExisting(Id);
                if (index == -1)
                {
                    List<Product> projuctLst = (List<Product>)(Session["Project"]);
                    Product product = projuctLst.Where(x => x.ProductId == Id).First();
                    cart.Add(new Item() { pr = product, Quantity = 1 });
                }
                else
                {
                    cart[index].Quantity++;

                }
                Session["cart"] = cart;
            }
            return View("Cart");
        }

        public ActionResult Update(FormCollection fc)
        {
            string[] quantities = fc.GetValues("quantity");
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                cart[i].Quantity = Convert.ToInt32(quantities[i]);
            }
            Session["cart"] = cart;
            return View("cart");
        }

        public ActionResult Checkout(FormCollection fc)
        {

            return View("Checkout");
        }

        public ActionResult SaveOrder(FormCollection fc)
        {
            if (IsCardNumberValid(NormalizeCardNumber(fc["cardNumber"])) )
            {
                bool res = false;
                switch(GetCardType(fc["cardNumber"]))
                {
                    case (CardType.Amex):
                        fc["cardType"] = "amex";
                        res = true;
                        break;
                    case (CardType.Discover):
                        fc["cardType"] = "discover";
                        res = true;
                     break;
                    case (CardType.MasterCard):
                        fc["cardType"] = "mastercard";
                        res = true;
                      break;
                    case (CardType.VISA):
                        fc["cardType"] = "visa";
                        res = true;
                       break;
                    default:
                        res = false;
                        break;
                }




                if (res)
                {
                    List<Item> cart = (List<Item>)Session["cart"];
                    Invoice invoice = new Invoice();
                    invoice.CustomerName = fc["customerName"];
                    invoice.CustomerAddress = fc["customerAddress"];
                    invoice.Name = "New Invoice";

                    PayPal.Api.Item paitem;
                    List<Item> lstItem = (List<Item>)Session["cart"];
                    List<PayPal.Api.Item> itms = new List<PayPal.Api.Item>();
                    PayPal.Api.ItemList itemList = new PayPal.Api.ItemList();
                    decimal totalPrice = 0;
                    foreach (var item in lstItem)
                    {
                        paitem = new PayPal.Api.Item();
                        paitem.name = item.pr.ProductName;
                        paitem.currency = "USD";
                        paitem.price = item.pr.Price.ToString();// "5";
                        paitem.quantity = "1";
                        paitem.sku = "sku";

                        if (item.Quantity > 0)
                        {
                            for (int i = 0; i < Convert.ToInt16(item.Quantity); i++)
                            {
                                itms.Add(paitem);

                            }
                            itemList.items = new List<PayPal.Api.Item>();
                            foreach (var itm in itms)
                            {

                                itemList.items.Add(itm);
                            }
                            
                            totalPrice = totalPrice + item.pr.Price * item.Quantity;
                        }
                       

                    }
                    //Now make a List of Item and add the above item to it
                    //you can create as many items as you want and add to this list


                    //Address for the payment
                    PayPal.Api.Address billingAddress = new PayPal.Api.Address();
                    billingAddress.city = "NewYork";
                    billingAddress.country_code = "US";
                    billingAddress.line1 = "23rd street kew gardens";
                    billingAddress.postal_code = "43210";
                    billingAddress.state = "NY";


                    //Now Create an object of credit card and add above details to it
                    //Please replace your credit card details over here which you got from paypal
                    PayPal.Api.CreditCard crdtCard = new PayPal.Api.CreditCard();
                    crdtCard.billing_address = billingAddress;
                    crdtCard.cvv2 = fc["CVV"];  //card cvv2 number
                    crdtCard.expire_month = Convert.ToInt32(fc["expMonth"]); //card expire date
                    crdtCard.expire_year = Convert.ToInt32(fc["expYear"]); //card expire year
                    crdtCard.first_name = fc["nameOnCardFirstName"];
                    crdtCard.last_name = fc["nameOnCardLastName"];

                    crdtCard.number = fc["cardNumber"]; //enter your credit card number here
                    crdtCard.type = fc["cardType"]; //credit card type here paypal allows 4 types

                    // Specify details of your payment amount.
                    PayPal.Api.Details details = new PayPal.Api.Details();
                    details.shipping = (1 * Convert.ToInt32(0)).ToString();
                    details.subtotal = totalPrice.ToString();
                    details.tax = (1 * Convert.ToInt32(0)).ToString();

                    // Specify your total payment amount and assign the details object
                    PayPal.Api.Amount amnt = new PayPal.Api.Amount();
                    amnt.currency = "USD";
                    // Total = shipping tax + subtotal.
                    amnt.total = (Convert.ToInt32(totalPrice) + Convert.ToInt32(details.shipping) + Convert.ToInt32(details.tax)).ToString();
                    amnt.details = details;

                    // Now make a transaction object and assign the Amount object
                    Random random = new Random();
                    PayPal.Api.Transaction tran = new PayPal.Api.Transaction();
                    tran.amount = amnt;
                    tran.description = "Description about the payment amount.";
                    tran.item_list = itemList;
                    tran.invoice_number = random.Next().ToString();

                    // Now, we have to make a list of transaction and add the transactions object
                    // to this list. You can create one or more object as per your requirements

                    List<PayPal.Api.Transaction> transactions = new List<PayPal.Api.Transaction>();
                    transactions.Add(tran);

                    // Now we need to specify the FundingInstrument of the Payer
                    // for credit card payments, set the CreditCard which we made above

                    PayPal.Api.FundingInstrument fundInstrument = new PayPal.Api.FundingInstrument();
                    fundInstrument.credit_card = crdtCard;

                    // The Payment creation API requires a list of FundingIntrument

                    List<PayPal.Api.FundingInstrument> fundingInstrumentList = new List<PayPal.Api.FundingInstrument>();
                    fundingInstrumentList.Add(fundInstrument);

                    // Now create Payer object and assign the fundinginstrument list to the object
                    PayPal.Api.Payer payr = new PayPal.Api.Payer();
                    payr.funding_instruments = fundingInstrumentList;
                    payr.payment_method = "credit_card";

                    // finally create the payment object and assign the payer object & transaction list to it
                    PayPal.Api.Payment pymnt = new PayPal.Api.Payment();
                    pymnt.intent = "sale";
                    pymnt.payer = payr;
                    pymnt.transactions = transactions;

                    try
                    {
                        //getting context from the paypal
                        //basically we are sending the clientID and clientSecret key in this function
                        //to the get the context from the paypal API to make the payment
                        //for which we have created the object above.

                        //Basically, apiContext object has a accesstoken which is sent by the paypal
                        //to authenticate the payment to facilitator account.
                        //An access token could be an alphanumeric string

                        PayPal.Api.APIContext apiContext = PayPalIntegration.Models.Configuration.GetAPIContext();

                        //Create is a Payment class function which actually sends the payment details
                        //to the paypal API for the payment. The function is passed with the ApiContext
                        //which we received above.

                        PayPal.Api.Payment createdPayment = pymnt.Create(apiContext);

                        //if the createdPayment.state is "approved" it means the payment was successful else not

                        if (createdPayment.state.ToLower() != "approved")
                        {
                            return View("FailureView");
                        }
                    }
                    catch (PayPal.PayPalException ex)
                    {
                        ViewBag.Error="Error: " + ex.Message;
                        return View("FailureView");
                    }
                    return View("Thanks");
                }
                else
                {
                    ViewBag.Error = "Please provide valid card";
                    return View("FailureView");
                }
                
            }
            else
            {
                ViewBag.Error = "Please provide valid credit card number";
                return View("FailureView");
            }

            
        }

        #region Private Methods
        public static bool IsCardNumberValid(string cardNumber)
        {
            int i, checkSum = 0;

            // Compute checksum of every other digit starting from right-most digit
            for (i = cardNumber.Length - 1; i >= 0; i -= 2)
                checkSum += (cardNumber[i] - '0');

            // Now take digits not included in first checksum, multiple by two,
            // and compute checksum of resulting digits
            for (i = cardNumber.Length - 2; i >= 0; i -= 2)
            {
                int val = ((cardNumber[i] - '0') * 2);
                while (val > 0)
                {
                    checkSum += (val % 10);
                    val /= 10;
                }
            }

            // Number is valid if sum of both checksums MOD 10 equals 0
            return ((checkSum % 10) == 0);
        }
        public static string NormalizeCardNumber(string cardNumber)
        {
            if (cardNumber == null)
                cardNumber = String.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (char c in cardNumber)
            {
                if (Char.IsDigit(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }
        // Array of CardTypeInfo objects.
        // Used by GetCardType() to identify credit card types.
        private static CardTypeInfo[] _cardTypeInfo =
        {
  new CardTypeInfo("^(51|52|53|54|55)", 16, CardType.MasterCard),
  new CardTypeInfo("^(4)", 16, CardType.VISA),
  new CardTypeInfo("^(4)", 13, CardType.VISA),
  new CardTypeInfo("^(34|37)", 15, CardType.Amex),
  new CardTypeInfo("^(6011)", 16, CardType.Discover),
  new CardTypeInfo("^(300|301|302|303|304|305|36|38)",
                   14, CardType.DinersClub),
  new CardTypeInfo("^(3)", 16, CardType.JCB),
  new CardTypeInfo("^(2131|1800)", 15, CardType.JCB),
  new CardTypeInfo("^(2014|2149)", 15, CardType.enRoute),
        };

        public static CardType GetCardType(string CardNumber)
        {
            
            foreach (CardTypeInfo info in _cardTypeInfo)
            {
                if (CardNumber.Length == info.Length &&
                    Regex.IsMatch(CardNumber, info.RegEx))
                    return info.Type;
            }

            return CardType.Unknown;
        }


        #endregion

    }



}