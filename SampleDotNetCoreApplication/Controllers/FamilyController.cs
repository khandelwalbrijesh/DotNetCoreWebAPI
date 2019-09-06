using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleDotNetCoreApplication.Models;
using SampleDotNetCoreApplication.View;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SampleDotNetCoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly FamilyContext _context;

        public FamilyController(FamilyContext context)
        {
            // this means we initialised the database here.
            this._context = context;
            if(_context.Families.Count() == 0)
            {
                Family defaultFamily = new Family();
                defaultFamily.HeadOfFamily = "Krishna";
                defaultFamily.LadyOfFamily = "Sunita";
                defaultFamily.FamilyName = "Khandelwal";
                defaultFamily.NumberOfFamilyMembers = 4;
                defaultFamily.FirstSonOfFamily = "Brijesh";
                defaultFamily.FirstDaughterOFFamily = "Ritu";
                _context.Families.Add(defaultFamily);
                _context.SaveChanges();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Family>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult  GetFamilies(string familyId)
        {
            List<Family> familyList = new List<Family>();
            foreach(var family in _context.Families)
            {
                familyList.Add(family);
            }

            if (familyList.Count == 0)
            {
                return new NotFoundObjectResult(new Error(StatusCodes.Status404NotFound, "No Family is not found."));
            }

            return Ok(familyList);
        }


        [HttpGet("FamilyId/{familyId}")]
        [ProducesResponseType(typeof(Family), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFamilyAsync(string familyId)
        {
            var family = await _context.Families.FindAsync(familyId);

            if(family == null)
            {
                return new NotFoundObjectResult(new Error(StatusCodes.Status404NotFound, "Given Family is not found."));
            }

            return Ok(family);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Family), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFamilyAsync( [FromBody] FamilyView family)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            Family familyModel = new Family(family);
            await _context.Families.AddAsync(familyModel);
            await _context.SaveChangesAsync();
            string guid = familyModel.FamilyId;
            return Ok(familyModel.FamilyId);
        }

        // It is a put request to update the some datamember of the family object.
        // But here client needs to send the complete object
        [HttpPut("familyid/{familyId}")]
        [ProducesResponseType(typeof(Family), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFamilyAsync(string familyId, [FromBody] FamilyView familyUpdate)
        {

            var family = await _context.Families.FindAsync(familyId);
            if(family == null)
            {
                return new NotFoundObjectResult(new Error(StatusCodes.Status404NotFound, "Given Family is not found."));
            }
            family.UpdateFamily(familyUpdate);
            _context.Entry(family).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpDelete("familyid/{familyId}")]
        [ProducesResponseType(typeof(Family), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Family), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveFamily(string familyId)
        {
            var family = await _context.Families.FindAsync(familyId);
            if(family == null)
            {
                return NotFound();
            }
            _context.Families.Remove(family);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
