using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleDotNetCoreApplication.Models;
using SampleDotNetCoreApplication.View;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

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
            string activityID = Guid.NewGuid().ToString();
            EventSourceLogging.Log.RequestLog(activityID, "GetFamilies", null);

            List<Family> familyList = new List<Family>();
            foreach(var family in _context.Families)
            {
                familyList.Add(family);
            }

            if (familyList.Count == 0)
            {
                string response = "No Family is not found.";
                EventSourceLogging.Log.RequestResponse(activityID, response);
                return new NotFoundObjectResult(new Error(StatusCodes.Status404NotFound, response));
            }

            EventSourceLogging.Log.RequestResponse(activityID, familyList.ToString());
            return Ok(familyList);
        }


        [HttpGet("FamilyId/{familyId}")]
        [ProducesResponseType(typeof(Family), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFamilyAsync(string familyId)
        {
            string activityID = Guid.NewGuid().ToString();
            EventSourceLogging.Log.RequestLog(activityID, "GetFamilyAsync", familyId);

            var family = await _context.Families.FindAsync(familyId);

            if(family == null)
            {
                string response = "Given Family is not found.";
                EventSourceLogging.Log.RequestResponse(activityID, response);
                return new NotFoundObjectResult(new Error(StatusCodes.Status404NotFound, response));
            }

            EventSourceLogging.Log.RequestResponse(activityID, family.ToString());
            return Ok(family);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Family), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFamilyAsync( [FromBody] FamilyView family)
        {
            string activityID = Guid.NewGuid().ToString();
            EventSourceLogging.Log.RequestLog(activityID, "AddFamilyAsync", family.ToString());
            if (!ModelState.IsValid)
            {                
                EventSourceLogging.Log.RequestResponse(activityID, "Invalid state");
                return BadRequest();
            }

            Family familyModel = new Family(family);
            await _context.Families.AddAsync(familyModel);
            await _context.SaveChangesAsync();
            string guid = familyModel.FamilyId;
            EventSourceLogging.Log.RequestResponse(activityID, guid);
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
            string activityID = Guid.NewGuid().ToString();
            EventSourceLogging.Log.RequestLog(activityID, "UpdateFamilyAsync", familyUpdate.ToString() + "-" + familyId);
            var family = await _context.Families.FindAsync(familyId);
            if(family == null)
            {
                string response = "Given Family is not found.";
                EventSourceLogging.Log.RequestResponse(activityID, response);
                return new NotFoundObjectResult(new Error(StatusCodes.Status404NotFound, response));
            }
            family.UpdateFamily(familyUpdate);
            _context.Entry(family).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            EventSourceLogging.Log.RequestResponse(activityID, "Success");
            return NoContent();
        }
        
        [HttpDelete("familyid/{familyId}")]
        [ProducesResponseType(typeof(Family), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Family), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveFamily(string familyId)
        {
            string activityID = Guid.NewGuid().ToString();
            EventSourceLogging.Log.RequestLog(activityID, "RemoveFamily", familyId);
            var family = await _context.Families.FindAsync(familyId);
            if(family == null)
            {
                EventSourceLogging.Log.RequestResponse(activityID, "204-Success");
                return NotFound();
            }
            _context.Families.Remove(family);
            await _context.SaveChangesAsync();
            EventSourceLogging.Log.RequestResponse(activityID, "201-Success");
            return Ok();
        }
    }
}
