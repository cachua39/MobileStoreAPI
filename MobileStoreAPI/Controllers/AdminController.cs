using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MobileStoreAPI.Models;
using MobileStoreAPI.Service;

namespace MobileStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly MobileStoreDBContext _context;
        private readonly IConfiguration _configuration;

        public AdminController(MobileStoreDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Admin
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
                    }).Where(op => op.MobileId == m.MobileId).ToList()
                }).ToListAsync();
            return Ok(listMobile);
        }

        // GET: api/Admin/5
        [HttpGet("{product}/{id}", Name = "Get")]
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
                    m.BrandId,
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
                    }).Where(op => op.MobileId == m.MobileId).ToList()
                }).Where(m => m.MobileId == id).SingleAsync();
            if (mobile == null)
            {
                return NotFound();
            }
            return Ok(mobile);
        }


        // PUT: api/Admin/5
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

        // POST: api/Admin
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("{product}")]
        public async Task<ActionResult<TblMobile>> PostTblMobile([FromForm]TblMobile tblMobile)
        {
            var brandId = await _context.TblBrand.Where(b => b.BrandName.Equals(Request.Form["BrandName"])).Select(b => b.BrandId).SingleAsync();
            
            ICollection<TblOption> tblOptions = new List<TblOption>();
            var listRam = Request.Form["RAM"].ToList<string>();
            var listMemory = Request.Form["Memory"].ToList<string>();
            var listColor = Request.Form["Color"].ToList<string>();
            var listQuantity = Request.Form["Quantity"].ToList<string>();
            var listPrice = Request.Form["ExtraPrice"].ToList<string>();
            TblOption tblOption = null;
            for (int i = 0; i < listRam.Count; i++)
            {
                tblOption = new TblOption
                {
                    MobileId = tblMobile.MobileId,
                    Ram = listRam[i],
                    Memory = listMemory[i],
                    Color = listColor[i],
                    Quantity = int.Parse(listQuantity[i]),
                    ExtraPrice = Math.Round(float.Parse(listPrice[i]), 2)
                };
                tblOptions.Add(tblOption);
            }

            // Upload file to server

            #region Read File Content  
            //var uploads = Path.Combine(env.WebRootPath, "uploads");
            //bool exists = Directory.Exists(uploads);
            //if (!exists)
            //{
            //    Directory.CreateDirectory(uploads);
            //}
            var phonePhotoFile = Request.Form.Files.GetFile("PhonePhoto");
            var fileName = Path.GetFileName(phonePhotoFile.FileName);

            //var photoPath = Path.Combine("uploads", phonePhotoFile.FileName);
            //var fileStream = new FileStream(photoPath, FileMode.Create);
            
            string mimeType = phonePhotoFile.ContentType;
            byte[] fileData = new byte[phonePhotoFile.Length];

            BlobStorageService objBlobService = new BlobStorageService(_configuration);

            tblMobile.Photo = objBlobService.UploadFileToBlob(phonePhotoFile);
            #endregion
            
            tblMobile.CreateDate = DateTime.Now;
            tblMobile.BrandId = brandId;
            tblMobile.Status = "Active";
            tblMobile.TblOption = tblOptions;

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

            return Created("", tblMobile);
        }

        // DELETE: api/Admin/5
        [HttpDelete("{product}/{id}")]
        public async Task<ActionResult<TblMobile>> DeleteTblMobile(string id)
        {
            var tblMobile = await _context.TblMobile.FindAsync(id);
            if (tblMobile == null)
            {
                return NotFound();
            }

            tblMobile.Status = "Inactive";

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

        private bool TblMobileExists(string id)
        {
            return _context.TblMobile.Any(e => e.MobileId == id);
        }
    }
}
