using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace surveychallenge.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        SurveyChallengeEntities db = new SurveyChallengeEntities();
        // GET: Survey
        public ActionResult Index()
        {
            try
            {
                return View(db.Survey.ToList());
            }
            catch (Exception)
            {
                return View();
            }
        }
        [Route("~/survey/create/{SurveyId}")]
        public ActionResult CreateSurvey(int SurveyId)
        {
            try
            {
                if (db.Survey.FirstOrDefault(x => x.SurveyId == SurveyId) != null)
                {
                    return View(db.Survey.FirstOrDefault(x => x.SurveyId == SurveyId));
                }
                else
                {
                    return View(new Survey());
                }
            }
            catch(Exception ex)
            {
                return View(new Survey());
            }

        }
        [HttpPost]
        public ActionResult SaveSurvey(Survey survey)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DeleteSurvey(survey.SurveyId);
                    db.Survey.AddOrUpdate(survey);
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { failure = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(new { failure = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteSurvey(int SurveyId)
        {
            try
            {
                if (db.Survey.FirstOrDefault(x => x.SurveyId == SurveyId) != null)
                {
                    foreach (Question question in db.Survey.FirstOrDefault(x => x.SurveyId == SurveyId).Question.ToList())
                    {
                        foreach (Option option in db.Survey.FirstOrDefault(x => x.SurveyId == SurveyId).Question.FirstOrDefault(y=>y.QuestionId==question.QuestionId).Option.ToList())
                        {
                            db.Option.Remove(db.Option.FirstOrDefault(x => x.OptionId == option.OptionId));
                        }
                        db.Question.Remove(db.Question.FirstOrDefault(x => x.QuestionId == question.QuestionId));
                    }
                    db.Survey.Remove(db.Survey.FirstOrDefault(x => x.SurveyId == SurveyId));
                    db.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { failure = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}