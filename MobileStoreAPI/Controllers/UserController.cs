using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileStoreAPI.Models;

namespace MobileStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MobileStoreDBContext _context;

        public UserController(MobileStoreDBContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet("{products}")]
        public async Task<ActionResult<IEnumerable<TblMobile>>> GetTblMobile()
        {
            var listMobile = await _context.TblMobile
                .Join(_context.TblBrand,
                m => m.BrandId,
                b => b.BrandId, (m, b) => new
                {
                    m.MobileId,
                    m.MobileName,
                    m.UnitPrice,
                    m.Photo,
                    m.Status,
                    b.BrandName
                }).Where(m => m.Status.Equals("Active") && 
                    _context.TblOption.Single(op => op.MobileId == m.MobileId && op.Quantity > 0) != null
                ).ToListAsync();
            return Ok(listMobile);
        }

        // GET: api/User/5
        [HttpGet("{product}/{id}")]
        public async Task<ActionResult<TblMobile>> GetTblMobile(string id)
        {
            var mobile = await _context.TblMobile
                .Join(_context.TblBrand,
                m => m.BrandId,
                b => b.BrandId, (m, b) => new
                {
                    m.MobileId,
                    m.MobileName,
                    m.UnitPrice,
                    m.Description,
                    m.Photo,
                    m.ScreenResolution,
                    m.ScreenSize,
                    m.OperatingSystem,
                    m.RearCamera,
                    m.FrontCamera,
                    m.Cpu,
                    m.BateryCapacity,
                    m.Sim,
                    m.Status,
                    m.CreateDate,
                    b.BrandName,
                    Options = _context.TblOption
                    .Select(op => new
                    {
                        op.MobileId,
                        op.Ram,
                        op.Memory,
                        op.Color,
                        op.Quantity,
                        op.ExtraPrice
                    }).Where(op => op.MobileId == m.MobileId && op.Quantity > 0).ToList()
                }).Where(m => m.Status.Equals("Active") && m.MobileId == id).SingleAsync();

            if(mobile == null)
            {
                return NotFound();
            }
            return Ok(mobile);
        }

        // PUT: api/User/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblMobile(string id, TblMobile tblMobile)
        {
            if (id != tblMobile.MobileId)
            {
                return BadRequest();
            }

            _context.Entry(tblMobile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblMobileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TblMobile>> PostTblMobile(TblMobile tblMobile)
        {
            _context.TblMobile.Add(tblMobile);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TblMobileExists(tblMobile.MobileId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTblMobile", new { id = tblMobile.MobileId }, tblMobile);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TblMobile>> DeleteTblMobile(string id)
        {
            var tblMobile = await _context.TblMobile.FindAsync(id);
            if (tblMobile == null)
            {
                return NotFound();
            }

            _context.TblMobile.Remove(tblMobile);
            await _context.SaveChangesAsync();

            return tblMobile;
        }

        private bool TblMobileExists(string id)
        {
            return _context.TblMobile.Any(e => e.MobileId == id);
        }
    }
}
