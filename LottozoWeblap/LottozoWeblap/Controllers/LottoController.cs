using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace LottozoWeblap.Controllers
{
    static class Filekezelo
    {
        const string cns = "server=localhost;port=3306;uid=root;database=lotto";

        public static List<string> Olvasas(Tipus tipus)
        {
            MySqlConnection db = new(cns);
            db.Open();

            int darab = tipus switch
            {
                Tipus.Otos => 5,
                Tipus.Hatos => 6,
                Tipus.Skandinav => 7,
                _ => throw new NotImplementedException(),
            };

            MySqlCommand cmd = db.CreateCommand();
            cmd.CommandText = $"SELECT * FROM `{tipus.ToString().ToLower()}`";

            List<string> szamok = [];

            MySqlDataReader dbolv = cmd.ExecuteReader();
            try
            {
                while (dbolv.Read())
                {
                    List<int> lekertSzamok = [];
                    for (int i = 0; i < darab; i++)
                    {
                        lekertSzamok.Add(dbolv.GetInt32(i));
                    }
                    szamok.Add(string.Join(", ", lekertSzamok));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                dbolv.Close();
                db.Close();
            }

            return szamok;
        }

        public static List<List<string>> MindentOlvasas()
        {
            List<List<string>> szamok = [];

            for (int i = 0; i < 3; i++)
            {
                szamok.Add(Olvasas((Tipus)i));
            }

            return szamok;
        }

        public static void Iras(Tipus tipus, int[] szamok)
        {
            MySqlConnection db = new(cns);
            db.Open();

            int darab = tipus switch
            {
                Tipus.Otos => 5,
                Tipus.Hatos => 6,
                Tipus.Skandinav => 7,
                _ => throw new NotImplementedException(),
            };

            MySqlCommand cmd = db.CreateCommand();
            cmd.CommandText = $"INSERT INTO `{tipus.ToString().ToLower()}` ({Formattalas('`', [.. Enumerable.Range(0, darab)])}) VALUES ({Formattalas('\'', szamok)});";

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                db.Close();
            }
        }

        static string Formattalas(char karakter, int[] szamok)
        {
            return string.Join(", ", szamok.Select(szam => $"{szam}"));
        }
    }

    [ApiController]
    [Route("/lotto")]
    public class LottoController : Controller
    {
        [HttpGet]
        public ActionResult<int[]> Get([FromQuery] Tipus? tipus)
        {
            if (!tipus.HasValue)
            {
                return Ok(Filekezelo.MindentOlvasas());
            }

            (int mennyit, int meddig) = tipus switch
            {
                Tipus.Otos => (5, 90),
                Tipus.Hatos => (6, 45),
                Tipus.Skandinav => (7, 35),
                _ => throw new NotImplementedException()
            };
            List<int> szamok = [];
            while (szamok.Count < mennyit)
            {
                int generalt = Random.Shared.Next(1, meddig + 1);
                if (!szamok.Contains(generalt))
                    szamok.Add(generalt);
            }
            szamok = [.. szamok.Order()];
            Filekezelo.Iras(tipus.Value, [.. szamok]);
            return Ok(szamok);
        }

    }
    public enum Tipus
    {
        Otos,
        Hatos,
        Skandinav
    }
}
