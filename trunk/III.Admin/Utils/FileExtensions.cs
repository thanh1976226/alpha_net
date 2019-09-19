using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
namespace ESEIM.Utils
{
    public static class FileExtensions
    {
        public static string ReadFileWord(string path)
        {
            string totaltext = "";
            using (var docs = DocumentFormat.OpenXml.Packaging.WordprocessingDocument.Open(path, false))
            {
                foreach (var el in docs.MainDocumentPart.Document.Body.Elements().OfType<Paragraph>())
                {
                    totaltext += " \r\n " + el.InnerText;
                }
            }
            return totaltext;
        }
        public static string ReadFileTxt(string path)
        {
            var result = File.ReadAllText(path, Encoding.UTF8);
            return result;
        }
        public static List<FileInfo> GetListFile(string dr, string fomart)
        {
            try
            {
                var dirInfo = new DirectoryInfo(dr);
                var file = (from f in dirInfo.GetFiles(fomart) select f).ToList();
                return file;
            }
            catch
            {
                return null;
            }
        }
        //[HttpPost]
        //public JsonResult GetIndex(IFormFile fileUpload)
        //{
        //    var msg = new JMessage() { Error = false, Title = "" };
        //    var upload = _upload.UploadFile(fileUpload, @"E:\LuceneFile");
        //    LuceneVersion MatchVersion = LuceneVersion.LUCENE_48;
        //    Lucene.Net.Store.Directory directory = FSDirectory.Open(new DirectoryInfo(@"E:\LuceneIndex"));
        //    var oAnalyzer = new StandardAnalyzer(MatchVersion);
        //    var oIndexWriterConfig = new IndexWriterConfig(MatchVersion, oAnalyzer);
        //    var oQueryParser = new MultiFieldQueryParser(MatchVersion, new[] { "name" }, oAnalyzer);
        //    var oIndexWriter = new IndexWriter(directory, oIndexWriterConfig);
        //    var oSearcherManager = new SearcherManager(oIndexWriter, true, null);
        //    //var oAdd = new Action<string>((sName) =>
        //    //{
        //    //    var oDocument = new Document
        //    //    {
        //    //        new Field("name", sName, Field.Store.YES,Field.Index.ANALYZED),
        //    //    };
        //    //    oIndexWriter.AddDocument(oDocument);
        //    //});
        //    Document doc = new Document();
        //    doc.Add(new TextField("name", fileUpload.FileName, Field.Store.YES));
        //    oIndexWriter.AddDocument(doc);
        //    oIndexWriter.Flush(true, true);
        //    //oIndexWriter.Commit();
        //    //oIndexWriter.ForceMerge(1);
        //    oIndexWriter.Dispose();
        //    return Json(msg);
        //}

        //[HttpPost]
        //public JsonResult GetIndexContent(IFormFile fileUpload)
        //{
        //    var msg = new JMessage() { Error = false, Title = "" };
        //    var upload = _upload.UploadFile(fileUpload, @"E:\LuceneFile");
        //    LuceneVersion MatchVersion = LuceneVersion.LUCENE_48;
        //    Lucene.Net.Store.Directory directory = FSDirectory.Open(new DirectoryInfo(@"E:\LuceneIndex"));
        //    var oAnalyzer = new StandardAnalyzer(MatchVersion);
        //    var oIndexWriterConfig = new IndexWriterConfig(MatchVersion, oAnalyzer);
        //    var oIndexWriter = new IndexWriter(directory, oIndexWriterConfig);
        //    var oSearcherManager = new SearcherManager(oIndexWriter, true, null);
        //    Document doc = new Document();
        //    doc.Add(new TextField("content", "a", Field.Store.YES));
        //    oIndexWriter.AddDocument(doc);
        //    oIndexWriter.Flush(true, true);
        //    oIndexWriter.Dispose();
        //    return Json(msg);
        //}

        //[HttpPost]
        //public JsonResult GetIndexWord(IFormFile fileUpload)
        //{
        //    var msg = new JMessage() { Error = false, Title = "" };
        //    var upload = _upload.UploadFile(fileUpload, @"E:\LuceneFile");
        //    LuceneVersion MatchVersion = LuceneVersion.LUCENE_48;
        //    Lucene.Net.Store.Directory directory = FSDirectory.Open(new DirectoryInfo(@"E:\LuceneIndex"));
        //    var oAnalyzer = new StandardAnalyzer(MatchVersion);
        //    var oIndexWriterConfig = new IndexWriterConfig(MatchVersion, oAnalyzer);
        //    //var oQueryParser = new MultiFieldQueryParser(MatchVersion, new[] { "name" }, oAnalyzer);
        //    var oIndexWriter = new IndexWriter(directory, oIndexWriterConfig);
        //    var oSearcherManager = new SearcherManager(oIndexWriter, true, null);
        //    Document doc = new Document();
        //    string path = @"E:\LuceneFile\" + upload.Object;
        //    var test = OfficeExtensions.ReadFileWord(path);
        //    doc.Add(new TextField("content", test, Field.Store.YES));
        //    oIndexWriter.AddDocument(doc);
        //    oIndexWriter.Flush(true, true);
        //    oIndexWriter.Dispose();
        //    return Json(msg);
        //}

        //[HttpPost]
        //public JsonResult SearchTest()
        //{
        //    var msg = new JMessage { Error = false, Title = "" };
        //    LuceneVersion MatchVersion = LuceneVersion.LUCENE_48;
        //    Lucene.Net.Store.Directory directory = FSDirectory.Open(new DirectoryInfo(@"E:\LuceneIndex"));
        //    var oAnalyzer = new StandardAnalyzer(MatchVersion);
        //    var oIndexWriterConfig = new IndexWriterConfig(MatchVersion, oAnalyzer);
        //    var oQueryParser = new MultiFieldQueryParser(MatchVersion, new[] { "content" }, oAnalyzer);
        //    var oIndexWriter = new IndexWriter(directory, oIndexWriterConfig);
        //    var oSearcherManager = new SearcherManager(oIndexWriter, true, null);
        //    var oSearch = new Action<string>((sQueryString) =>
        //    {
        //        var oQuery = oQueryParser.Parse(sQueryString);
        //        oSearcherManager.MaybeRefreshBlocking();
        //        var oSearcher = oSearcherManager.Acquire();

        //        try
        //        {
        //            var oTopDocs = oSearcher.Search(oQuery, 10);
        //            var nTotalHits = oTopDocs.TotalHits;
        //            Console.WriteLine("Total Hits: {0}", nTotalHits);

        //            foreach (var oResult in oTopDocs.ScoreDocs)
        //            {
        //                var oDocument = oSearcher.Doc(oResult.Doc);

        //                var nScore = oResult.Score;
        //                var sName = oDocument.GetField("content")?.GetStringValue();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //        }
        //        finally
        //        {
        //            oSearcherManager.Release(oSearcher);
        //            oSearcher = null;
        //        }
        //    });
        //    oSearch("I-20");
        //    oIndexWriter.Dispose();
        //    return Json(msg);
        //}
    }
}
