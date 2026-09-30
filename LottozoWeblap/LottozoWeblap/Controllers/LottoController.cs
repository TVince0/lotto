using Microsoft.AspNetCore.Mvc;

namespace Lottozo.Controllers
{
    [ApiController]
    [Route("/lotto")]
    public class LottoController : Controller
    {
        [HttpPost]
        public ActionResult<int[]> Post([FromBody] Tipus tipus)
        {
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
