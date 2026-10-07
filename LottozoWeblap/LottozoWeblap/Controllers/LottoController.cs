using Microsoft.AspNetCore.Mvc;

namespace LottozoWeblap.Controllers
{
    static class Filekezelo
    {
        public static List<List<string>> Olvasas()
        {
            int index = 0;
            List<List<string>> szamok = [[], [], []];

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "szamok.txt");
            if (Path.Exists(path))
            {
                string[] sorok = System.IO.File.ReadAllLines(path);

                foreach (string sor in sorok)
                {
                    if (string.IsNullOrWhiteSpace(sor))
                    {
                        index++;
                        continue;
                    }

                    szamok[index].Add(sor);
                }
            }

            return szamok;
        }

        public static void Iras(int index, string szamok)
        {
            List<List<string>> EddigiSzamok = Olvasas();
            EddigiSzamok[index].Add(szamok);

            int elozoI = 0;

            StreamWriter sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "szamok.txt"));
            for (int i = 0; i < EddigiSzamok.Count; i++)
            {
                if (elozoI != i)
                {
                    elozoI = i;
                    sw.WriteLine();
                }

                for (int j = 0; j < EddigiSzamok[i].Count; j++)
                {
                    sw.WriteLine(EddigiSzamok[i][j]);
                }
            }
            sw.Close();
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
                return Ok(Filekezelo.Olvasas());
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
            Filekezelo.Iras((int)tipus, string.Join(", ", szamok));
            return Ok(szamok);
        }

        public enum Tipus
        {
            Otos,
            Hatos,
            Skandinav
        }
    }
}
